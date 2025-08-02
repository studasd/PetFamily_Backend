using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.IDs;
public record PetId(Guid Value) : IComparable<PetId>
{
	public static PetId NewPeetId() => new(Guid.NewGuid());
	public static PetId Empty() => new(Guid.Empty);
	public static PetId Create(Guid guid) => new(guid);

	public static implicit operator PetId(Guid id) => new(id);
	public static implicit operator Guid(PetId id)
	{
		ArgumentNullException.ThrowIfNull(id);
		return id.Value;
	}

	public int CompareTo(PetId? vol)
	{
		if (vol is null)
			throw new ArgumentException("PeetId is not null");

		return Value.CompareTo(vol.Value);
	}
}