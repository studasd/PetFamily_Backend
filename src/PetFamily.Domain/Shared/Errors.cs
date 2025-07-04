﻿
namespace PetFamily.Domain.Shared;

public static class Errors
{
	public static class General
	{
		public static Error ValueIsInvalid(string name) => 
			Error.Validation("value_is_invalid", $"'{name ?? "value"}' is invalid");

		public static Error NotFound(Guid? id = null) =>
			Error.NotFound("record_not_found", $"Record not found for id '{(id is null ? "" : $" for Id '{id}'")}'");

		public static Error ValueIsRequired(string name) => 
			Error.Validation("value_is_required", $"'{(name is null ? "" : $" {name} ")}' is required");
	}
}