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
using Twilio.Types;

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

        public async Task<ListCustomerInforModel?> searchCustomerAsync(string token, string inputText, int pageIndex, int pageSize)
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
                parameters.Add("@text", inputText);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output); // Thêm tham số đầu ra

                var result = await connection.QueryAsync<CustomerInforModel>(
                       "searchCustomer",
                       parameters,
                       commandType: CommandType.StoredProcedure
                   );
                int totalRecord = parameters.Get<int>("@TotalRecord");
                return new ListCustomerInforModel
                {
                    ListCustomerInfor = result.ToList(),
                    TotalRecord = totalRecord
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ListCustomerInforModel> getAllCustomersAsync(string token, string status,int gender, int pageIndex, int pageSize)
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
                var parametersCustomerInfor = new DynamicParameters();
                parametersCustomerInfor.Add("@status", status);
                parametersCustomerInfor.Add("@gender", gender);
                parametersCustomerInfor.Add("@PageIndex", pageIndex);
                parametersCustomerInfor.Add("@PageSize", pageSize);

                var parameterstotalRecord = new DynamicParameters();
                parameterstotalRecord.Add("@gender", gender);
                parameterstotalRecord.Add("@status", status);

                var customerInfor = await connection.QueryAsync<CustomerInforModel>(
                       "getAllCustomers",
                       parametersCustomerInfor,
                       commandType: CommandType.StoredProcedure
                   );
                var totalRecord = await connection.QueryFirstOrDefaultAsync<int>(
                       "getTotalCustomersNumber",
                       parameterstotalRecord,
                       commandType: CommandType.StoredProcedure
                   );
                return new ListCustomerInforModel
                {
                    ListCustomerInfor = customerInfor.ToList(),
                    TotalRecord = totalRecord
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
