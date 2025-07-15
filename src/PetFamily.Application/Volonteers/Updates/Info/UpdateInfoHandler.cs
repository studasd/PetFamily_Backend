using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Volonteers.Updates.Info;

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

		var nameReq = request.UpdateInfoDTO.Name;
		var name = VolunteerName.Create(nameReq.Firstname, nameReq.Lastname, nameReq.Surname).Value;

		volunteerResult.Value.UpdateInfo(name, request.UpdateInfoDTO.Email, request.UpdateInfoDTO.Description);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Updated volunteer {name}, {email}, {description} with id {volunteerId}", name, request.UpdateInfoDTO.Email, request.UpdateInfoDTO.Description, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}
