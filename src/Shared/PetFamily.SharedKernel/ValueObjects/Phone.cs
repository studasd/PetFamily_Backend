using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.SharedKernel.ValueObjects;

public record Phone(string PhoneNumber)
{
	public static Result<Phone, Error> Create(string phone)
	{
		if (string.IsNullOrWhiteSpace(phone))
			return Errors.General.ValueIsRequired("Phone");

		var checkPhone = Regex.Match(phone, @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$");
		if (!checkPhone.Success)
			return Errors.General.ValueIsInvalid("Phone");

		return new Phone(phone);
	}
}
