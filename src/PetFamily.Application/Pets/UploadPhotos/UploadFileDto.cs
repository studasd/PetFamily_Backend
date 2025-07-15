namespace PetFamily.Application.Pets.UploadPhotos;

public record UploadFileDto(Stream Content, string FileName, string ContentType);