using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.Volonteers;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.API.Examples;

public class VolunteerRequestExample : IExamplesProvider<CreateVolunteerRequest>
{
	public CreateVolunteerRequest GetExamples()
	{
		var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		return new CreateVolunteerRequest(
			name: new NameDTO(Firstname: guid.Substring(0, 5), Lastname: guid.Substring(5, 5), Surname: guid.Substring(10, 5)),
			email: $"{guid.Substring(4, 4)}@{guid.Substring(8, 3)}.{guid.Substring(10, 2)}",
			description: guid.Substring(5, 15),
			experienceYears: Random.Shared.Next(10),
			phone: $"{Random.Shared.NextInt64(79014445865, 79994445865)}", 
			bankingDetails: null, 
			socialNetworks: null
		);
	}
}