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

	public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
	{
		this.minioClient = minioClient;
		this.logger = logger;
	}


	public async Task<Result<string, Error>> UploadFileAsync(FileData fileData, CancellationToken token = default)
	{
		try
		{
			var bucketExistArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);
			var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, token);
			if (bucketExist == false)
			{
				var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);

				await minioClient.MakeBucketAsync(makeBucketArgs, token);
			}


			var putObjectArgs = new PutObjectArgs()
				.WithBucket(fileData.BucketName)
				.WithStreamData(fileData.Stream)
				.WithObjectSize(fileData.Stream.Length)
				.WithObject(fileData.FileName);

			var result = await minioClient.PutObjectAsync(putObjectArgs, token);

			return result.ObjectName;
		}
		catch (Exception e)
		{
			logger.LogError(e, "Fail to upload file in minio");
			return Error.Failure("file_upload", "Fail to upload file in minio");
		}
	}
}
