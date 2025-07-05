
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer;

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

		var volunteer = Volunteer.Create(volunteerName.Value, request.Email, request.Description, request.ExperienceYears, request.Phone);

		if (volunteer.IsFailure)
			return volunteer.Error;

		await volunteerRepository.AddAsync(volunteer.Value, token);

		return volunteer.Value.Id.Value;
	}
}
