using PetFamily.Contracts.Pets;
using PetFamily.Domain.VolunteerManagement.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.API.Examples;

public class PetRequestExample : IExamplesProvider<CreatePetRequestDTO>
{
	public CreatePetRequestDTO GetExamples()
	{
		var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		return new CreatePetRequestDTO(
			Name: guid.Substring(0, 5),
			Type: PetTypes.Dog,
			Description: guid.Substring(5, 15),
			Breed: guid.Substring(8, 7),
			Species: guid.Substring(11, 5),
			Color: guid.Substring(10, 7),
			Weight: Random.Shared.Next(1, 70),
			Height: Random.Shared.Next(1, 20),
			Phone: $"{Random.Shared.NextInt64(79014445865, 79994445865)}",
			HelpStatus: PetHelpStatuses.NeedsHelp,
			AddressDTO: new CreatePetRequestAddressDTO(
				Country: "Russia",
				City: "Tobok",
				Street: "Mira",
				HouseNumber: 5,
				Apartment: 45
			)
		);
	}
}