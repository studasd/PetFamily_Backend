using PetFamily.Core.Abstractions;

namespace PetFamily.Application.PetsManagement.Commands.UpdatePrimePhoto;

public record UpdatePetPrimePhotoCommand(Guid VolunteerId, Guid PetId, string PathPhoto) : ICommand;