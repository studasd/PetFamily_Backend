using Microsoft.AspNetCore.Mvc;
using Minio;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
	private readonly IFileProvider minioProvider;

	public FileController(IFileProvider minioProvider)
	{
		this.minioProvider = minioProvider;
	}


	[HttpPost]
	public async Task<IActionResult> CreateFile(IFormFile file, CancellationToken token)
	{
		//// список buckets
		//var buckets = await minioClient.ListBucketsAsync(token);
		//var bucketsStr = String.Join(",", buckets.Buckets.Select(b => b.Name));

		await using var stream = file.OpenReadStream();

		var fn = Guid.NewGuid();

		var fd = new FileUploadData(stream, "photos", fn.ToString());

		var result = await minioProvider.UploadFileAsync(fd, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}


	[HttpDelete("{bucketname}/{filename}")]
	public async Task<IActionResult> DeleteFile(
		[FromRoute] string bucketname,
		[FromRoute] Guid filename,
		CancellationToken token)
	{
		var fd = new FileData(bucketname, filename.ToString());

		var result = await minioProvider.DeleteFileAsync(fd, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result);
	}


	[HttpGet("{bucketname}/{filename}")]
	public async Task<IActionResult> PresignedFile(
		[FromRoute] string bucketname,
		[FromRoute] Guid filename,
		CancellationToken token)
	{
		var fd = new FileData(bucketname, filename.ToString());

		var result = await minioProvider.PresignedFileAsync(fd, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
