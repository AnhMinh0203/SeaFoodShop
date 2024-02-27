using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SeaFoodShop.DataContext.Data
{
    public class ConnectToSql
    {
        public readonly IConfiguration _configuration ;
        public string? ConnectString { get; }
        public ConnectToSql(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectString = _configuration.GetConnectionString("SeaFoodShop");
        }
        public IDbConnection CreateConnection() => new SqlConnection(ConnectString);
    }
}
