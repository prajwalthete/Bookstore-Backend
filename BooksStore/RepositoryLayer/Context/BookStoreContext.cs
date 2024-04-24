using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace RepositoryLayer.Context
{
    public class BookStoreContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BookStoreContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("BooksStoreConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);


    }
}
