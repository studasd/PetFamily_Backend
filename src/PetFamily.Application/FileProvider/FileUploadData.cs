namespace PetFamily.Application.FileProvider;

public record FileUploadData(Stream Stream, string BucketName, string FileName);
