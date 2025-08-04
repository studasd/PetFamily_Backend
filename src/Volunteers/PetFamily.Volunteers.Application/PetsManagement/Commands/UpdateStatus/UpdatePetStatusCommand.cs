using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, PetHelpStatuses HelpStatus) : ICommand;