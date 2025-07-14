using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(FileData fileData, CancellationToken token);
	Task<Result<string, Error>> PresignedFileAsync(FileData fileData, CancellationToken token);
	Task<UnitResult<Error>> UploadFilesAsync(FileUploadData fileData, CancellationToken token);
}
