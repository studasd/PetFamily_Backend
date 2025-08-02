using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public abstract class AbsSoftDeletableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
	protected AbsSoftDeletableEntity(TId id) : base(id)
	{
		
	}

	public bool IsSoftDeleted {  get; private set; }

	public DateTime? DateDeletion {  get; private set; }


	public virtual void Delete()
	{
		if (IsSoftDeleted)
			return;

		IsSoftDeleted = true;
		DateDeletion = DateTime.UtcNow;
	}

	public virtual void Restore()
	{
		if (IsSoftDeleted == false)
			return;

		IsSoftDeleted = false;
		DateDeletion = null;
	}
}
