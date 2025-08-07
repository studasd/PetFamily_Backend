using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.RequestVolonteers;
using Swashbuckle.AspNetCore.Filters;

namespace PetFamily.Volunteers.Presentation.Examples;

public class VolunteerRequestExample : IExamplesProvider<CreateVolunteerRequest>
{
	public CreateVolunteerRequest GetExamples()
	{
		var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();

		return new CreateVolunteerRequest(
			Name: new NameDTO(Firstname: guid.Substring(0, 5), Lastname: guid.Substring(5, 5), Surname: guid.Substring(10, 5)),
			Email: $"{guid.Substring(4, 4)}@{guid.Substring(8, 3)}.{guid.Substring(10, 2)}",
			Description: guid.Substring(5, 15),
			ExperienceYears: Random.Shared.Next(10),
			Phone: $"{Random.Shared.NextInt64(79014445865, 79994445865)}", 
			BankingDetails: null
		);
	}
}