using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;
