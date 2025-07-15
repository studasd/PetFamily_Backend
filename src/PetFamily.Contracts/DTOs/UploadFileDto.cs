namespace PetFamily.Contracts.DTOs;

public record UploadFileDto(Stream Content, string FileName, string ContentType);