using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.PetEntities;
public record PetId(Guid Value) : IComparable<PetId>
{
	public static implicit operator Guid(PetId petId)
	{
		ArgumentNullException.ThrowIfNull(petId);
		return petId.Value;
	}

	public static PetId NewPeetId() => new(Guid.NewGuid());
	public static PetId Empty() => new(Guid.Empty);
	public static PetId Create(Guid guid) => new(guid);

	public int CompareTo(PetId? vol)
	{
		if (vol is null)
			throw new ArgumentException("PeetId is not null");

		return Value.CompareTo(vol.Value);
	}
}