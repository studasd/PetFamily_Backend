using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Update;

public class UpdateInfoHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<UpdateInfoHandler> logger;

	public UpdateInfoHandler(IVolunteerRepository volunteerRepository, ILogger<UpdateInfoHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(UpdateInfoRequest request, CancellationToken token = default)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(request.VolunteerId, token);

		if(volunteerResult.IsFailure)
			return volunteerResult.Error;

		return Guid.Empty;
	}
}
