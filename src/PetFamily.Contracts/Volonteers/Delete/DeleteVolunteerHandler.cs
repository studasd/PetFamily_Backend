using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Contracts.Volonteers.Update;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Delete;

public class DeleteVolunteerHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<DeleteVolunteerHandler> logger;

	public DeleteVolunteerHandler(IVolunteerRepository volunteerRepository, ILogger<DeleteVolunteerHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(DeleteVolunteerRequest request, CancellationToken token = default)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(request.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error;

		await volunteerRepository.DeleteAsync(volunteerResult.Value, token);

		logger.LogInformation("Deleted volunteer with id {volunteerId}", volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}
