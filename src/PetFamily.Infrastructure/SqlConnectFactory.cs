using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application.Database;
using System.Data;

namespace PetFamily.Infrastructure;

public class SqlConnectFactory : ISqlConnectFactory
{
	private readonly IConfiguration configuration;

	public SqlConnectFactory(IConfiguration configuration)
	{
		this.configuration = configuration;
	}


	public IDbConnection Create() =>
		new NpgsqlConnection(configuration.GetConnectionString("Database"));
}
