namespace PetFamily.SharedKernel;

public class Error
{
	public const string SEPARATOR = "||";

	private Error(string code, string message, ErrorTypes type, string? invalidField = null)
	{
		Code = code;
		Message = message;
		Type = type;
		InvalidField = invalidField;
	}

	public string Code { get; }
	public string Message { get; }
	public ErrorTypes Type { get; }
	public int TypeCode => (int)Type;
	public string? InvalidField { get; } = null;


	public static Error Validation(string code, string message, string? invalidField = null) => 
		new(code, message, ErrorTypes.Validation, invalidField);
	public static string ValidationSerialize(string code, string message) => Validation(code, message).Serialize();

	public static Error NotFound(string code, string message) => new(code, message, ErrorTypes.NotFound);
	public static string NotFoundSerialize(string code, string message) => NotFound(code, message).Serialize();

	public static Error Failure(string code, string message) => new(code, message, ErrorTypes.Failure);
	public static string FailureSerialize(string code, string message) => Failure(code, message).Serialize();

	public static Error Conflict(string code, string message) => new(code, message, ErrorTypes.Conflict);
	public static string ConflictSerialize(string code, string message) => Conflict(code, message).Serialize();

	public ErrorList ToErrorList() => new([this]);


	public string Serialize() => string.Join(SEPARATOR, Code, Message, Type);


	public static Error Deserialize(string serialaze)
	{
		var parts = serialaze.Split(SEPARATOR);

		if (parts.Length < 3)
		{
			throw new ArgumentException("Invalid serialized format");
		}

		if (!Enum.TryParse<ErrorTypes>(parts[2], out var type))
		{
			throw new ArgumentException("Invalid serialized format");
		}

		return new(parts[0], parts[1], type);
	}
}
