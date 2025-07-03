using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public record Phone(string phone)
{
	public static Result<Phone> Create(string phone)
	{
		if (string.IsNullOrWhiteSpace(phone))
			return Result.Failure<Phone>("Phone cannot be empty");

		var checkPhone = Regex.Match(phone, @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$");
		if (!checkPhone.Success)
			return Result.Failure<Phone>("Error in the phone number format");

		return new Phone(phone);
	}
}
