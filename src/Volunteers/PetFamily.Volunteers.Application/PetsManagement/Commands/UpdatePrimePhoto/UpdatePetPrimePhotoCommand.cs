using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdatePrimePhoto;

public record UpdatePetPrimePhotoCommand(Guid VolunteerId, Guid PetId, string PathPhoto) : ICommand;