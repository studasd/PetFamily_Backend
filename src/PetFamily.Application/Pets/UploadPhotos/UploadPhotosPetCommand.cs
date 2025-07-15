using PetFamily.Contracts.DTOs;

namespace PetFamily.Application.Pets.UploadPhotos;

public record UploadPhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> UploadFiles);
