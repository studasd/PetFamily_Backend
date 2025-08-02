using PetFamily.Contracts.DTOs;
using PetFamily.Core.Abstractions;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.Info;

public record UpdateInfoCommand(Guid VolunteerId, NameDTO Name, string Email, string Description) : ICommand;
