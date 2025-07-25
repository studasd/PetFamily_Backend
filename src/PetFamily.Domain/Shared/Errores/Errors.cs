﻿namespace PetFamily.Domain.Shared.Errores;

public static class Errors
{
	public static class General
	{
		public static Error ValueIsInvalid(string name) => 
			Error.Validation("value_is_invalid", $"'{name ?? "value"}' is invalid");

		public static Error NotFound(Guid? id = null) =>
			Error.NotFound("record_not_found", $"Record not found '{(id is null ? "" : $"for Id '{id}'")}'");

		public static Error NotFound(string name) =>
			Error.NotFound("record_not_found", $"Record not found for name '{name}'");

		public static Error ValueIsRequired(string name) => 
			Error.Validation("value_is_required", $"'{(name is null ? "" : $" {name} ")}' is required");

		public static Error AlreadyExist(string name) =>
			Error.Validation("record_already_exist", $"'{name}' already exist");

		public static Error AlreadyIsUsed(string name) =>
			Error.Validation("record_already_used", $"'{name}' already used");
	}
}