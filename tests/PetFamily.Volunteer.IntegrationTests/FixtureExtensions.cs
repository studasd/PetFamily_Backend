using AutoFixture;
using PetFamily.Application.PetsManagement.Commands.Add;

namespace PetFamily.Volunteer.IntegrationTests;

public static class FixtureExtensions
{
	public static AddPetCommand CreateAddPetCommand(this IFixture fixture, Guid volunteerId, Guid speciesId, Guid breedId)
	{
		return fixture.Build<AddPetCommand>()
			.With(x => x.VolunteerId, volunteerId)
			.With(x => x.Height, 7)
			.With(x => x.Weight, 15)
			.With(x => x.Phone, "546868498864")
			.With(x => x.Color, "color")
			.With(x => x.SpeciesId, speciesId)
			.With(x => x.BreedId, breedId)
			.Create();
	}
}
