using System.Data.Entity;

namespace StarterKit.DataAccess.Context
{
	public class PostgresDbConfiguration : DbConfiguration
	{
		public PostgresDbConfiguration()
		{
			SetDefaultConnectionFactory(new Npgsql.NpgsqlConnectionFactory());
			SetProviderFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
			SetProviderServices("Npgsql", Npgsql.NpgsqlServices.Instance);
		}
	}
}