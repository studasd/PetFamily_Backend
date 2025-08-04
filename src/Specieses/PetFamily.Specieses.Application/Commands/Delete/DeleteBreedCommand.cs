using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Commands.Delete;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
