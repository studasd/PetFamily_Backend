namespace PetFamily.Application.Pets.UploadPhotos;

public record UploadFileCommand(Stream Stream, string FileName, string ContentType);