using PetFamily.Contracts.DTOs;

namespace PetFamily.Contracts.Volonteers.Updates.Info;

public record UpdateInfoRequestDTO(NameDTO Name, string Email, string Description);