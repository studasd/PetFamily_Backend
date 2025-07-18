using System.Data;

namespace PetFamily.Application.Database;

public interface ISqlConnectFactory
{
	IDbConnection Create();
}