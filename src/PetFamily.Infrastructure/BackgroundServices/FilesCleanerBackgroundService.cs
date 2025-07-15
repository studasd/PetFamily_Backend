using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
	private readonly ILogger<FilesCleanerBackgroundService> logger;
	private readonly IServiceScopeFactory scopeFactory;
	private readonly IMessageQueue<IEnumerable<FileInform>> messageQueue;

	public FilesCleanerBackgroundService(
		ILogger<FilesCleanerBackgroundService> logger, 
		IServiceScopeFactory scopeFactory,
		IMessageQueue<IEnumerable<FileInform>> messageQueue)
	{
		this.logger = logger;
		this.scopeFactory = scopeFactory;
		this.messageQueue = messageQueue;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using var scope = scopeFactory.CreateAsyncScope();

		var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

		while (!stoppingToken.IsCancellationRequested)
		{
			var fileInfos = await messageQueue.ReadAsync(stoppingToken);

			foreach(var fileInfo in fileInfos)
			{
				await fileProvider.DeleteFileAsync(fileInfo, stoppingToken);
			}

			logger.LogInformation("FilesCleanerBackgroundService is started");

			//await Task.Delay(3000, stoppingToken);
		}

		await Task.CompletedTask;
	}
}
