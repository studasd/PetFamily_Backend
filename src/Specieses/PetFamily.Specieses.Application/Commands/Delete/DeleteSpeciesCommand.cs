using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;
