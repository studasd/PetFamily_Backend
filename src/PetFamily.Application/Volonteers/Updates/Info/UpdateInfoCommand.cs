using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.RequestVolonteers;

namespace PetFamily.Application.Volonteers.Updates.Info;

public record UpdateInfoCommand(Guid VolunteerId, NameDTO Name, string Email, string Description);
