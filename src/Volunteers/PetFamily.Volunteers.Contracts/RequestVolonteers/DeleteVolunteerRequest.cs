namespace PetFamily.Volunteers.Contracts.RequestVolonteers;

public record DeleteVolunteerRequest(Guid VolunteerId, bool IsSoftDelete);
