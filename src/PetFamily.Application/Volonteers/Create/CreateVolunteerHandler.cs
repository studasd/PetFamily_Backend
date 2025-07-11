
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Volonteers.Create;

public class CreateVolunteerHandler // CreateVolunteerService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<CreateVolunteerHandler> logger;

	public CreateVolunteerHandler(IVolunteerRepository volunteerRepository, ILogger<CreateVolunteerHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(CreateVolunteerRequest request, CancellationToken token = default)
	{
		var volunteerName = VolunteerName.Create(request.Name.Firstname, request.Name.Lastname, request.Name.Surname).Value;
		
		var volunteerNameExist = await volunteerRepository.GetByNameAsync(volunteerName, token);
		
		if (volunteerNameExist.IsSuccess)
			return Errors.General.AlreadyExist("Volunteer");

		var phone = Phone.Create(request.Phone).Value;

		var volunteer = Volunteer.Create(volunteerName, request.Email, request.Description, request.ExperienceYears, phone).Value;

		if (request.SocialNetworks.Count() > 0)
		{
			var socNetworksResult = request.SocialNetworks.Select(s => SocialNetwork.Create(s.Name, s.Link).Value);
			volunteer.AddSocialNetworks(socNetworksResult);
		}

		if (request.BankingDetails.Count() > 0)
		{
			var bankDetailsResult = request.BankingDetails.Select(s => BankingDetails.Create(s.Name, s.Description).Value);
			volunteer.AddBankingDetails(bankDetailsResult);
		}

		await volunteerRepository.AddAsync(volunteer, token);

		logger.LogInformation("Created volunteer {volunteerName} with id {volunteerId}", volunteerName, volunteer.Id);

		return volunteer.Id.Value;
	}
}
