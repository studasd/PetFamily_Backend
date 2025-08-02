using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.MovePosition;

public record MovePositionPetCommand(Guid VolunteerId, Guid PetId, int NewPosition) : ICommand;
