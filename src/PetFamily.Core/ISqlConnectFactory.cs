using System.Data;

namespace PetFamily.Core;

public interface ISqlConnectFactory
{
	IDbConnection Create();
}