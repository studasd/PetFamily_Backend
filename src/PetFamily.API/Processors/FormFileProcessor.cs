
using PetFamily.Application.Pets.UploadPhotos;

namespace PetFamily.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
	private readonly List<UploadFileDto> uploadFileDtos = [];

	public List<UploadFileDto> Process(IEnumerable<IFormFile> files)
	{
		foreach (var file in files)
		{
			var stream = file.OpenReadStream();

			var fileDto = new UploadFileDto(stream, file.FileName, file.ContentType);
			uploadFileDtos.Add(fileDto);
		}

		return uploadFileDtos;
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var file in uploadFileDtos) 
		{
			await file.Content.DisposeAsync();
		}
	}
}
