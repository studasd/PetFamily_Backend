namespace PetFamily.Application.Volonteers.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId, bool IsSoftDelete);
