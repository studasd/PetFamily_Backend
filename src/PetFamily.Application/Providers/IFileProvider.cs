using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(FileData fileData, CancellationToken token = default);
	Task<Result<string, Error>> PresignedFileAsync(FileData fileData, CancellationToken token = default);
	Task<Result<string, Error>> UploadFileAsync(FileUploadData fileData, CancellationToken token = default);
}
