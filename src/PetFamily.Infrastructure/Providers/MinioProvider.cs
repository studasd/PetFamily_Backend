using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
	private readonly IMinioClient minioClient;
	private readonly ILogger<MinioProvider> logger;
	const int MAX_THREAD_UPLOAD_FILES = 10;

	public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
	{
		this.minioClient = minioClient;
		this.logger = logger;
	}


	public async Task<UnitResult<Error>> UploadFilesAsync(FileUploadData fileData, CancellationToken token = default)
	{
		var semaphoreSlim = new SemaphoreSlim(MAX_THREAD_UPLOAD_FILES);

		try
		{
			var bucketExistArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);
			var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, token);
			if (bucketExist == false)
			{
				var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);

				await minioClient.MakeBucketAsync(makeBucketArgs, token);
			}

			List<Task> tasks = [];
			foreach (var file in fileData.Files)
			{
				await semaphoreSlim.WaitAsync(token);

				var putObjectArgs = new PutObjectArgs()
					.WithBucket(fileData.BucketName)
					.WithStreamData(file.Stream)
					.WithObjectSize(file.Stream.Length)
					.WithObject(file.FileName);

				var task = minioClient.PutObjectAsync(putObjectArgs, token);

				semaphoreSlim.Release();

				tasks.Add(task);
			}
			await Task.WhenAll(tasks);
		}
		catch (Exception e)
		{
			logger.LogError(e, "Fail to upload file in minio");
			return Error.Failure("file_upload", "Fail to upload file in minio");
		}
		finally
		{
			semaphoreSlim.Release();
		}

		return Result.Success<Error>();
	}


	public async Task<UnitResult<Error>> DeleteFileAsync(FileData fileData, CancellationToken token = default)
	{
		try
		{
			var bucketRemoveArgs = new RemoveObjectArgs()
				.WithBucket(fileData.BucketName)
				.WithObject(fileData.FileName);

			await minioClient.RemoveObjectAsync(bucketRemoveArgs);

			return UnitResult.Success<Error>();
		}
		catch (Exception e)
		{
			logger.LogError(e, "Fail to delete file in minio");
			return Error.Failure("file_delete", "Fail to delete file in minio");
		}
	}


	public async Task<Result<string, Error>> PresignedFileAsync(FileData fileData, CancellationToken token = default)
	{
		try
		{
			var bucketPresignedArgs = new PresignedGetObjectArgs()
		   .WithBucket(fileData.BucketName)
		   .WithObject(fileData.FileName)
		   .WithExpiry(1000);
			
			var bucketPresigned = await minioClient.PresignedGetObjectAsync(bucketPresignedArgs);

			return bucketPresigned;
		}
		catch (Exception e)
		{
			logger.LogError(e, "Fail to presigned file in minio");
			return Error.Failure("file_presigned", "Fail to presigned file in minio");
		}
	}
}
