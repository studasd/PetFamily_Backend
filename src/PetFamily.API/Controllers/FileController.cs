using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
	private readonly IMinioClient minioClient;

	public FileController(IMinioClient minioClient)
	{
		this.minioClient = minioClient;
	}


	[HttpPost]
	public async Task<IActionResult> CreateFile(IFormFile file, CancellationToken token)
	{
		//// список buckets
		//var buckets = await minioClient.ListBucketsAsync(token);
		//var bucketsStr = String.Join(",", buckets.Buckets.Select(b => b.Name));

		var path = Guid.NewGuid();

		var bucketExistArgs = new BucketExistsArgs().WithBucket("photo");
		var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, token);
		if(bucketExist == false)
		{
			var makeBucketArgs = new MakeBucketArgs().WithBucket("photo");

			await minioClient.MakeBucketAsync(makeBucketArgs, token);
		}

		 
		await using var stream = file.OpenReadStream();

		var putObjectArgs = new PutObjectArgs()
			.WithBucket("photos")
			.WithStreamData(stream)
			.WithObjectSize(stream.Length)
			.WithObject(file.FileName);

		var result = await minioClient.PutObjectAsync(putObjectArgs, token);

		return Ok(result);
	}
}
