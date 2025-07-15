using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public class Position : ValueObject
{
	private Position(int value)
	{
		Value = value;
	}

	public int Value { get; }

	public static Position First = new(1);

	public static implicit operator int(Position position) => position.Value;
	//public static implicit operator Position(int value) => new(value); // не сработает валидация

	public Result<Position, Error> Forward() => Create(Value + 1);
	public Result<Position, Error> Back() => Create(Value - 1);

	public static Result<Position, Error> Create(int number)
	{
		if (number <= 0)
			return Errors.General.ValueIsInvalid("serial number");

		return new Position(number);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
