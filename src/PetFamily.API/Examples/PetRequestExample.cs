using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.Enums;
using PetFamily.Contracts.RequestPets;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.API.Examples;

public class PetRequestExample : IExamplesProvider<AddPetRequest>
{
	public AddPetRequest GetExamples()
	{
		var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		return new AddPetRequest(
			Name: guid.Substring(0, 5),
			Description: guid.Substring(5, 15),
			BreedId: Guid.NewGuid(),
			SpeciesId: Guid.NewGuid(),
			Color: guid.Substring(10, 7),
			Weight: Random.Shared.Next(1, 70),
			Height: Random.Shared.Next(1, 20),
			Phone: $"{Random.Shared.NextInt64(79014445865, 79994445865)}",
			HelpStatus: PetHelpStatuses.NeedsHelp,
			AddressDTO: new AddressDTO(
				Country: "Russia",
				City: "Tobok",
				Street: "Mira",
				HouseNumber: 5,
				Apartment: 45
			)
		);
	}
}