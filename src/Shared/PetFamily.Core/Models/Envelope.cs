﻿using PetFamily.SharedKernel;

namespace PetFamily.Core.Models;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);


public record Envelope
{
	public object? Result { get; }
	public ErrorList? Errors { get; }
	public DateTime TimeGenerated { get; }

	public Envelope(object? result, ErrorList? errors)
	{
		Result = result;
		Errors = errors;
		TimeGenerated = DateTime.UtcNow;
	}


	public static Envelope Ok(object? result = null) => new(result, null);
	
	public static Envelope Error(ErrorList errors) => new(null, errors);
}
