using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.FileProvider;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(FileInform fileData, CancellationToken token);
	Task<Result<string, Error>> PresignedFileAsync(FileInform fileData, CancellationToken token);
	Task<Result<IReadOnlyList<string>, Error>> UploadFilesAsync(IEnumerable<FileData> fileDatas, CancellationToken token);
}
