using PetFamily.Core.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Commands.Delete;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
