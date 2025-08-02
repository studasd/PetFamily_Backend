namespace PetFamily.Volunteers.Domain.SpeciesManagement.IDs;

public record SpeciesId(Guid Value) : IComparable<SpeciesId>
{
	public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
	public static SpeciesId Empty() => new(Guid.Empty);
	public static SpeciesId Create(Guid guid) => new(guid);

	public static implicit operator SpeciesId(Guid id) => new(id);
	public static implicit operator Guid(SpeciesId id)
	{
		ArgumentNullException.ThrowIfNull(id);
		return id.Value;
	}

	public int CompareTo(SpeciesId? vol)
	{
		if (vol is null)
			throw new ArgumentException("SpeciesId is not null");

		return Value.CompareTo(vol.Value);
	}
}