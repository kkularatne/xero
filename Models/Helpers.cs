using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace RefactorThis.Models
{
    public interface IHelpers
    {
        SqliteConnection NewConnection();
    }

    public class Helpers : IHelpers
    {
        private readonly IConfiguration _configuration;

        public Helpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // private const string ConnectionString =  //"Data Source=App_Data/products.db";

        public SqliteConnection NewConnection()
        {
            return new SqliteConnection(this._configuration.GetConnectionString("xero"));
        }
    }
}