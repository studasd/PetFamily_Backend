using PetFamily.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.PetsManagement.Commands.UploadPhotos;

public record UploadPhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> UploadFiles) : ICommand;
