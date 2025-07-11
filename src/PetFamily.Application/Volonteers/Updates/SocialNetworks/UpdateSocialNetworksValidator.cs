using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.Volonteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.Updates.SocialNetworks;

public class UpdateSocialNetworksValidator : AbstractValidator<UpdateSocialNetworksRequest>
{
	public UpdateSocialNetworksValidator()
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));
	}
}
