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

		var fileName = Guid.NewGuid();

		var fileData = new FileUploadData(stream, "photos", fileName.ToString());

		var result = await minioProvider.UploadFileAsync(fileData, token);
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
		var fileData = new FileData(bucketname, filename.ToString());

		var result = await minioProvider.DeleteFileAsync(fileData, token);
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
		var fileData = new FileData(bucketname, filename.ToString());

		var result = await minioProvider.PresignedFileAsync(fileData, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
