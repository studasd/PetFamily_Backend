namespace PetFamily.API.Responses;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);


public record Envelop
{
	public object? Result { get; }
	public List<ResponseError> Errors { get; }
	public DateTime TimeGenerated { get; }

	public Envelop(object? result, IEnumerable<ResponseError> errors)
	{
		Result = result;
		Errors = errors.ToList();
		TimeGenerated = DateTime.Now;
	}


	public static Envelop Ok(object? result = null) => new(result, []);
	
	public static Envelop Error(IEnumerable<ResponseError> errors) => new(null, errors);
}
