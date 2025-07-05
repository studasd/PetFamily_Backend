namespace PetFamily.Domain.Shared;

public class Error
{
	private Error(string code, string message, ErrorTypes type)
	{
		Code = code;
		Message = message;
		Type = type;
	}

	public string Code { get; }
	public string Message { get; }
	public ErrorTypes Type { get; }
	public int TypeCode => (int)Type;


	public static Error Validation(string code, string message) => new(code, message, ErrorTypes.Validation);

	public static Error NotFound(string code, string message) => new(code, message, ErrorTypes.NotFound);

	public static Error Failure(string code, string message) => new(code, message, ErrorTypes.Failure);

	public static Error Conflict(string code, string message) => new(code, message, ErrorTypes.Conflict);
}
