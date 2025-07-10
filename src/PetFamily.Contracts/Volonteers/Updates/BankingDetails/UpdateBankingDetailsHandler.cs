using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Updates.BankingDetails;

public class UpdateBankingDetailsHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<UpdateBankingDetailsHandler> logger;

	public UpdateBankingDetailsHandler(IVolunteerRepository volunteerRepository, ILogger<UpdateBankingDetailsHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(UpdateBankingDetailsRequest request, CancellationToken token = default)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(request.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error;

		var bankingDetailsResult = request.BankingDetailsDTO.BankingDetails.Select(s => Domain.Shared.ValueObjects.BankingDetails.Create(s.Name, s.Description).Value);

		volunteerResult.Value.UpdateBankingDetails(bankingDetailsResult);

		await volunteerRepository.SaveAsync();

		logger.LogInformation("Updated volunteer banking details {bankings} with id {volunteerId}", bankingDetailsResult, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}