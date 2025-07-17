using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsManagement.Commands.MovePosition;

public record MovePositionPetCommand(Guid VolunteerId, Guid PetId, int NewPosition) : ICommand;
