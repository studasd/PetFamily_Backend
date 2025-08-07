using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.AccountManagement.UseCases.Updates.SocialNetworks;

public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
	private readonly IAccountRepository accountRepository;
	private readonly IValidator<UpdateSocialNetworksCommand> validator;
	private readonly ILogger<UpdateSocialNetworksHandler> logger;

	public UpdateSocialNetworksHandler(
		IAccountRepository accountRepository, 
		IValidator<UpdateSocialNetworksCommand> validator,
		ILogger<UpdateSocialNetworksHandler> logger)
	{
		this.accountRepository = accountRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdateSocialNetworksCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var accountResult = await accountRepository.GetByIdAsync(command.UserId, token);

		if (accountResult.IsFailure)
			return accountResult.Error.ToErrorList();

		var socNetworksResult = command.SocialNetworks.Select(s => SocialNetwork.Create(s.Name, s.Link).Value);

		accountResult.Value.UpdateSocialNetworks(socNetworksResult);

		await accountRepository.SaveAsync(token);

		logger.LogInformation("Updated account social networks {socials} with id {accountId}", socNetworksResult, accountResult.Value.Id);

		return accountResult.Value.Id;
	}
}