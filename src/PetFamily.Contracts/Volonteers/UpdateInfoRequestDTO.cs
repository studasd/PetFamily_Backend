using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers;

public record UpdateInfoRequestDTO(NameDTO Name, string Email, string Description);