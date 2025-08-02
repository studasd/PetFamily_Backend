﻿using Microsoft.AspNetCore.Authorization;

namespace PetFamily.API.Authorization;

public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
	public PermissionAttribute(string code) : base(policy: code)
	{
		Code = code;
	}

	public string Code { get; set; }
}
