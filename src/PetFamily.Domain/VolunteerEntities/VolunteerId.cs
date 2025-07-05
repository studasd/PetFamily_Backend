namespace PetFamily.Domain.VolunteerEntities;

public record VolunteerId(Guid Value) : IComparable<VolunteerId>
{
	public static implicit operator Guid(VolunteerId volunteerId)
	{
		ArgumentNullException.ThrowIfNull(volunteerId);
		return volunteerId.Value;
	}

	public static VolunteerId NewVolunteerId()	=> new(Guid.NewGuid());
	public static VolunteerId Empty()			=> new(Guid.Empty);
	public static VolunteerId Create(Guid guid) => new(guid);

	public int CompareTo(VolunteerId? vol)
	{
		if (vol is null) 
			throw new ArgumentException("VolunteerId is not null");

		return Value.CompareTo(vol.Value);
	}
}