namespace PetFamily.Volunteers.Contracts.DTOs;

public record UploadFileDto(Stream Content, string FileName, string ContentType);