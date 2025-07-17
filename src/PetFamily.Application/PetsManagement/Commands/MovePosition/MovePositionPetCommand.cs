namespace PetFamily.Application.PetsManagement.Commands.MovePosition;

public record MovePositionPetCommand(Guid VolunteerId, Guid PetId, int NewPosition);
