namespace PetFamily.Application.Pets.MovePosition;

public record MovePositionPetCommand(Guid VolunteerId, Guid PetId, int NewPosition);
