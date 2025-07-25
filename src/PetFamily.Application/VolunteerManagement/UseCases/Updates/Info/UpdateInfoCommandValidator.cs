﻿using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.Info;

public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
{
	public UpdateInfoCommandValidator() 
	{
		RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer Id is not empty"));

		RuleFor(c => c.Name)
			.MustBeValueObject(x => VolunteerName.Create(x.Firstname, x.Lastname, x.Surname));

		RuleFor(c => c.Email)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Email is not empty"))
			.EmailAddress().WithError(Errors.General.ValueIsInvalid("Email not valid"));

		RuleFor(c => c.Description)
			.NotEmpty().WithError(Errors.General.ValueIsRequired("Description is not empty"))
			.MaximumLength(100).WithError(Errors.General.ValueIsInvalid("Description maximum lenght: 100"));
	}
}
