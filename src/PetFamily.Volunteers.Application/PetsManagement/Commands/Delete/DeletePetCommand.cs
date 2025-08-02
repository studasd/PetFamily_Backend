using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.Delete;

public record DeletePetCommand(Guid VolunteerId, Guid PetId, bool IsSoftDelete) : ICommand;
