﻿namespace PetFamily.Framework.Authorization;

public static class Permissions
{
	public static class Accounts
	{
		public const string CreatePet = "pet.create";
		public const string UpdatePet = "pet.update";
		public const string DeletePet = "pet.delete";
		public const string ReadPet =	"pet.read";
	}
}
