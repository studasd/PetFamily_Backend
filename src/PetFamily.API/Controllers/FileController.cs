using Microsoft.AspNetCore.Mvc;
using Minio;
using PetFamily.Core.Extensions;
using PetFamily.Core.FileProvider;

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

		List<FileData> fileDatas = [new FileData(stream, new FileInform(fileName.ToString(), "photos"))];

		var result = await minioProvider.UploadFilesAsync(fileDatas, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok();
	}


	[HttpDelete("{bucketname}/{filename}")]
	public async Task<IActionResult> DeleteFile(
		[FromRoute] string bucketname,
		[FromRoute] Guid filename,
		CancellationToken token)
	{
		var fileData = new FileInform(bucketname, filename.ToString());

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
		var fileData = new FileInform(bucketname, filename.ToString());

		var result = await minioProvider.PresignedFileAsync(fileData, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
