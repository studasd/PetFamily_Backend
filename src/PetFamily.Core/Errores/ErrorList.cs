using System.Collections;

namespace PetFamily.Core.Errores;

public class ErrorList : IEnumerable<Error>
{
	private readonly List<Error> errors = [];

	public ErrorList(IEnumerable<Error> errors)
	{
		this.errors = [..errors];
	}


	public IEnumerator<Error> GetEnumerator()
	{
		return errors.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public static implicit operator ErrorList(List<Error> errors) => new(errors);
	public static implicit operator ErrorList(Error error) => new([error]);
}
