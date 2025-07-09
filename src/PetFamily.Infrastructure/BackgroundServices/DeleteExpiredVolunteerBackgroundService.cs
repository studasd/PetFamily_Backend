using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure.BackgroundServices;

public class DeleteExpiredVolunteerBackgroundService : BackgroundService
{
	private readonly ILogger<DeleteExpiredVolunteerBackgroundService> logger;
	private readonly IServiceScopeFactory serviceScopeFactory;

	public DeleteExpiredVolunteerBackgroundService(ILogger<DeleteExpiredVolunteerBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
	{
		this.logger = logger;
		this.serviceScopeFactory = serviceScopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken token)
	{
		logger.LogInformation("DeleteExpiredVolunteerBackgroundService start");

		while (!token.IsCancellationRequested)
		{
			logger.LogInformation("DeleteExpiredVolunteerService work");

			await using var scope = serviceScopeFactory.CreateAsyncScope();

			var serviceVolunteer = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteerService>();

			await serviceVolunteer.StartAsync(token);

			await Task.Delay(TimeSpan.FromHours(Constants.SOFT_DELETING_HOUR), token);
		}
	}
}
