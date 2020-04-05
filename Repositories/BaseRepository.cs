using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace RefactorThis.Repositories
{

    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;

        protected BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqliteConnection NewConnection()
        {
            return new SqliteConnection(this._configuration.GetConnectionString("xero"));
        }
    }
}
