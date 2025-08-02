using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.DeletePhotos;

public record DeletePhotosPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> DeleteFiles) : ICommand;
