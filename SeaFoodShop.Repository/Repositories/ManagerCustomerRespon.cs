using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Repositories
{
    public class ManagerCustomerRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;

        public ManagerCustomerRespon(ConnectToSql context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        // Add customer (like sign up)
        // Update customer (like updateUserProfileAsync)
        public async Task<string> deleteCustomerAsync(string token, string phoneNumber)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return "Vui lòng đăng nhập";
            }
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@phoneNumber", phoneNumber);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "deleteCustomer",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    return parameters.Get<string>("@result");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<CustomerInforModel>?> searchCustomerAsync(string token, string phoneNumber, string status)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return null;
            }
            try
            {
                using var connection = (SqlConnection)_context.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@phoneNumber", phoneNumber);
                parameters.Add("@status", status);

                var result = await connection.QueryAsync<CustomerInforModel>(
                       "searchCustomer",
                       parameters,
                       commandType: CommandType.StoredProcedure
                   );
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
