namespace PetFamily.Contracts.Volonteers;

public record DeleteVolunteerRequest(Guid VolunteerId, bool IsSoftDelete);
