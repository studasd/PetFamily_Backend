using FluentValidation;
using PetFamily.Contracts.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

public class UpdateSocialNetworksValidator : AbstractValidator<UpdateSocialNetworksRequest>
{
	public UpdateSocialNetworksValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
