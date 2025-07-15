using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.RequestVolonteers;

public record UpdateInfoRequest(NameDTO Name, string Email, string Description);