
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Contracts.Volonteers.Create;

public class CreateVolunteerHandler // CreateVolunteerService
{
	private readonly IVolunteerRepository volunteerRepository;

	public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
	{
		this.volunteerRepository = volunteerRepository;
	}

	public async Task<Result<Guid, Error>> HandleAsync(CreateVolunteerRequest request, CancellationToken token = default)
	{
		var volunteerName = VolunteerName.Create(request.Firstname, request.Lastname, request.Surname);
		
		if (volunteerName.IsFailure)
			return volunteerName.Error;

		var volunteerNameExist = await volunteerRepository.GetByNameAsync(volunteerName.Value, token);
		
		if (volunteerNameExist.IsSuccess)
			return Errors.General.AlreadyExist("Volunteer");

		var phone = Phone.Create(request.Phone);

		if (phone.IsFailure)
			return phone.Error;

		var volunteer = Volunteer.Create(volunteerName.Value, request.Email, request.Description, request.ExperienceYears, phone.Value);

		if (volunteer.IsFailure)
			return volunteer.Error;

		if (request.SocialNetworks.Count() > 0)
		{
			foreach (var network in request.SocialNetworks)
				volunteer.Value.AddSocialNetwork(network.Name, network.Link);
		}

		if (request.BankingDetails is not null)
			volunteer.Value.AddBankingDetails(request.BankingDetails.Name, request.BankingDetails.Description);

		await volunteerRepository.AddAsync(volunteer.Value, token);

		return volunteer.Value.Id.Value;
	}
}
