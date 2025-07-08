using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Update;

public record UpdateInfoRequestDTO(NameDTO Name, string Email, string Description);