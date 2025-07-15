namespace PetFamily.Application.FileProvider;

public record FileData(Stream Content, string FileName, string BucketName);
