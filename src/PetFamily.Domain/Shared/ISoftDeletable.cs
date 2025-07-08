namespace PetFamily.Domain.Shared;

public interface ISoftDeletable
{
	bool IsHardDelete { get; }
	void Delete();
	void Restore();
}