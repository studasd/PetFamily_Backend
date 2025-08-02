using PetFamily.Core.Abstractions;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.PetsManagement.Commands.UpdateStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, PetHelpStatuses HelpStatus) : ICommand;