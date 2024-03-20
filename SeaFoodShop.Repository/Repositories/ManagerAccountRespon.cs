using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace SeaFoodShop.Repository.Repositories
{
    public class ManagerAccountRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;

        public ManagerAccountRespon(ConnectToSql context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        public async Task<string> LockAccountAsync(string phoneNumber)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@phoneNumber", phoneNumber);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "lockAccount",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    return parameters.Get<string>("@result");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> UnlockAccountAsync(string phoneNumber)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@phoneNumber", phoneNumber);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "unLockAccount",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    return parameters.Get<string>("@result");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AccountModel>?> SearchAccountAsync(string token, string phoneNumber, string status)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null) return null;
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var listAccount = await connection.QueryAsync<AccountModel>(
                        "searchAccount",
                        new { phoneNumber, status },
                        commandType: CommandType.StoredProcedure
                    );

                    return listAccount.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> changePasswordAdminAsync(string token, ChangePasswordAdminModel password)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
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
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string newHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                            using (var changePasswordCommand = new SqlCommand("ChangePasswordAdmin", connection, transaction))
                            {
                                changePasswordCommand.CommandType = CommandType.StoredProcedure;
                                changePasswordCommand.Parameters.AddWithValue("@phoneNumber", password.PhoneNumber);
                                changePasswordCommand.Parameters.AddWithValue("@newPassword", newHash);
                                await changePasswordCommand.ExecuteNonQueryAsync();

                                transaction.Commit();
                                return "Cập nhật mật khẩu thành công";
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
