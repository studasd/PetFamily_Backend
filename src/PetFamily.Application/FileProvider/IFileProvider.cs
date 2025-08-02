using CSharpFunctionalExtensions;
using PetFamily.Core.Errores;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(FileInform fileData, CancellationToken token);
	Task<Result<string, Error>> PresignedFileAsync(FileInform fileData, CancellationToken token);
	Task<Result<IReadOnlyList<string>, Error>> UploadFilesAsync(IEnumerable<FileData> fileDatas, CancellationToken token);
}
