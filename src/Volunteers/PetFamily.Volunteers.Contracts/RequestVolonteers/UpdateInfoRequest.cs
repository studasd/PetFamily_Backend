using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.RequestVolonteers;

public record UpdateInfoRequest(NameDTO Name, string Email, string Description);