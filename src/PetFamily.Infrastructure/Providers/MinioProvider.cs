using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

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


	public async Task<Result<IReadOnlyList<string>, Error>> UploadFilesAsync(IEnumerable<FileData> fileDatas, CancellationToken token)
	{
		var semaphoreSlim = new SemaphoreSlim(MAX_THREAD_UPLOAD_FILES);
		var filesList = fileDatas.ToList();

		try
		{
			var buckets = fileDatas.Select(file => file.FileInform.BucketName);

			await IfBucketsNotExistCreateBucket(buckets, token);

			var tasks = filesList.Select(async file =>
				await PutObject(file, semaphoreSlim, token));

			var pathResult = await Task.WhenAll(tasks);

			if (pathResult.Any(p => p.IsFailure))
				return pathResult.First().Error;

			var results = pathResult.Select(p => p.Value).ToList();

			return results;
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
	}


	private async Task<Result<string, Error>> PutObject(FileData fileData, SemaphoreSlim semaphoreSlim, CancellationToken token)
	{
		await semaphoreSlim.WaitAsync(token);

		var putObjectArgs = new PutObjectArgs()
			.WithBucket(fileData.FileInform.BucketName)
			.WithStreamData(fileData.Content)
			.WithObjectSize(fileData.Content.Length)
			.WithObject(fileData.FileInform.FileName);

		try
		{
			await minioClient.PutObjectAsync(putObjectArgs, token);

			return fileData.FileInform.FileName;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Fail to upload file in minio with path {path} in bucket {bucket}", fileData.FileInform.FileName, fileData.FileInform.BucketName);

			return Error.Failure("file_upload", "Fail to upload file in minio");
		}
		finally
		{
			semaphoreSlim.Release();
		}
	}

	private async Task IfBucketsNotExistCreateBucket(IEnumerable<string> fileDatas, CancellationToken token)
	{
		HashSet<string> bucketNames = [..fileDatas];

		foreach (var bucketName in bucketNames)
		{
			var bucketExistArgs = new BucketExistsArgs()
				.WithBucket(bucketName);

			var bucketExists = await minioClient.BucketExistsAsync(bucketExistArgs, token);

			if (bucketExists == false)
			{
				var makeBucketArgs = new MakeBucketArgs()
					.WithBucket(bucketName);

				await minioClient.MakeBucketAsync(makeBucketArgs, token);
			}
		}
	}
	


	public async Task<UnitResult<Error>> DeleteFileAsync(FileInform fileData, CancellationToken token)
	{
		try
		{
			await IfBucketsNotExistCreateBucket([fileData.BucketName], token);

			var statArgs = new StatObjectArgs()
				.WithBucket(fileData.BucketName)
				.WithObject(fileData.FileName);

			var objectStat = await minioClient.StatObjectAsync(statArgs, token);

			if (objectStat == null)
				return Result.Success<Error>();

			var bucketRemoveArgs = new RemoveObjectArgs()
				.WithBucket(fileData.BucketName)
				.WithObject(fileData.FileName);

			await minioClient.RemoveObjectAsync(bucketRemoveArgs);

			return UnitResult.Success<Error>();
		}
		catch (Exception e)
		{
			logger.LogError(e, "Fail to delete file in minio with {path} in bucket {bucket}", fileData.FileName, fileData.BucketName);
			return Error.Failure("file_delete", "Fail to delete file in minio");
		}
	}


	public async Task<Result<string, Error>> PresignedFileAsync(FileInform fileData, CancellationToken token)
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
