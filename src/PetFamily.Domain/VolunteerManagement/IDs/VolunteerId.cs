﻿namespace PetFamily.Domain.VolunteerManagement.IDs;

public record VolunteerId(Guid Value) : IComparable<VolunteerId>
{
	public static VolunteerId NewVolunteerId()	=> new(Guid.NewGuid());
	public static VolunteerId Empty()			=> new(Guid.Empty);
	public static VolunteerId Create(Guid guid) => new(guid);

	public static implicit operator VolunteerId(Guid id) => new(id);
	public static implicit operator Guid(VolunteerId volunteerId)
	{
		ArgumentNullException.ThrowIfNull(volunteerId);
		return volunteerId.Value;
	}

	public int CompareTo(VolunteerId? vol)
	{
		if (vol is null) 
			throw new ArgumentException("VolunteerId is not null");

		return Value.CompareTo(vol.Value);
	}
}