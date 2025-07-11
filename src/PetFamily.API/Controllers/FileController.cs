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

		var fd = new FileData(stream, "photos", file.FileName);

		var result = await minioProvider.UploadFileAsync(fd, token);
		if (result.IsFailure)
			return result.Error.ToResponse();

		return Ok(result.Value);
	}
}
