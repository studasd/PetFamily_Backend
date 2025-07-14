using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(FileInform fileData, CancellationToken token);
	Task<Result<string, Error>> PresignedFileAsync(FileInform fileData, CancellationToken token);
	Task<UnitResult<Error>> UploadFilesAsync(IEnumerable<FileData> fileDatas, CancellationToken token);
}
