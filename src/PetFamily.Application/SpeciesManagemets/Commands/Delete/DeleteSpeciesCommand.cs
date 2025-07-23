using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;
