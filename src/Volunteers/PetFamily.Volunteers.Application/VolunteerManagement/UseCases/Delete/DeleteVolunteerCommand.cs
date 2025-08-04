using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.UseCases.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId, bool IsSoftDelete) : ICommand;
