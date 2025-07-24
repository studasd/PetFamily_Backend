using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsManagement.Commands.Delete;

public record DeletePetCommand(Guid VolunteerId, Guid PetId, bool IsSoftDelete) : ICommand;
