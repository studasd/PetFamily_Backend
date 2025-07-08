using FluentValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Contracts.Volonteers.Update;

public class UpdateInfoValidator : AbstractValidator<UpdateInfoRequest>
{
	public UpdateInfoValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithMessage(Error.NotFoundSerialize("id_invalid", "Volunteer Id is not empty"));	
	}
}