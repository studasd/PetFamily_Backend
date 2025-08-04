namespace PetFamily.Specieses.Domain.IDs;

public record BreedId(Guid Value) : IComparable<BreedId>
{
	public static BreedId NewBreedId() => new(Guid.NewGuid());
	public static BreedId Empty() => new(Guid.Empty);
	public static BreedId Create(Guid guid) => new(guid);

	public static implicit operator BreedId(Guid id) => new(id);
	public static implicit operator Guid(BreedId id)
	{
		ArgumentNullException.ThrowIfNull(id);
		return id.Value;
	}

	public int CompareTo(BreedId? vol)
	{
		if (vol is null)
			throw new ArgumentException("BreedId is not null");

		return Value.CompareTo(vol.Value);
	}
}
