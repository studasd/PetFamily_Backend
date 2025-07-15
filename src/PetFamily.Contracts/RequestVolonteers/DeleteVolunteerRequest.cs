namespace PetFamily.Contracts.RequestVolonteers;

public record DeleteVolunteerRequest(Guid VolunteerId, bool IsSoftDelete);
