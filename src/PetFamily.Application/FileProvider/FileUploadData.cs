namespace PetFamily.Application.FileProvider;

public record FileUploadData(IEnumerable<FileContent> Files, string BucketName);
