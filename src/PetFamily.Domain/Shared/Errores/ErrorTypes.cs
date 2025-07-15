namespace PetFamily.Domain.Shared.Errores;

public enum ErrorTypes
{
	Failure		= 500,
	Validation	= 400,
	NotFound	= 404,
	Conflict	= 409
}