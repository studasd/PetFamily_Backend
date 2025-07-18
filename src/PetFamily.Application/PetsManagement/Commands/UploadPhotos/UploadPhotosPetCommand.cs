using PetFamily.Application.Abstractions;
using PetFamily.Contracts.DTOs;

namespace PetFamily.Application.PetsManagement.Commands.UploadPhotos;

public record UploadPhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> UploadFiles) : ICommand;
