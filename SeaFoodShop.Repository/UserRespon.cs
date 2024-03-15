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

namespace SeaFoodShop.Repository
{
    public class UserRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public UserRespon (ConnectToSql context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserProfileModel?> getUserProfile (string token)
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
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUser", idUser);
                    var userProfile = await connection.QueryFirstOrDefaultAsync<UserProfileModel>(
                        "GetProfile",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return userProfile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> updateUserProfileAsync(string token,UserProfileModel user)
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
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUser", idUser);
                    parameters.Add("@fullName", user.FullName);
                    parameters.Add("@dob", user.Dob);
                    parameters.Add("@gender", user.Gender);
                    parameters.Add("@phoneNumber", user.PhoneNumber);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "UpdateProfile",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    string result = parameters.Get<string>("@result");
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);    
            }
        }
        // Cách 1
        /*public async Task<string> changePasswordAsync (string token, ChangePasswordModel password)
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
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Generate a salt for bcrypt encryption
                    string hash = BCrypt.Net.BCrypt.HashPassword(password.OldPassword, salt);
                    using (var command = new SqlCommand("getPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUser", idUser);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                string hashedPassword = reader.GetString(reader.GetOrdinal("Password"));
                                // Verify the input password against the hashed password using bcrypt
                                bool passwordMatch = BCrypt.Net.BCrypt.Verify(password.OldPassword, hashedPassword);
                                if (passwordMatch)
                                {
                                    reader.Close();
                                    string newSalt = BCrypt.Net.BCrypt.GenerateSalt(); // Generate a salt for bcrypt encryption
                                    string newHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword, salt);

                                    var parameters = new DynamicParameters();
                                    parameters.Add("@idUser", idUser);
                                    parameters.Add("@newPassword", newHash);
                                    parameters.Add("@result", "", DbType.String, ParameterDirection.Output);
                                    await connection.ExecuteAsync("ChangePassword", parameters, commandType: CommandType.StoredProcedure);

                                    string resultMessage = parameters.Get<string>("@result");

                                    return resultMessage;
                                }
                                else
                                {
                                    return "Mật khẩu không đúng";
                                }
                            }
                            return "";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }*/
        // Cách 2
        public async Task<string> changePasswordAsync(string token, ChangePasswordModel password)
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
                    using(var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string? currentPassword;
                            using (var getPasswordCommand = new SqlCommand("GetPassword", connection, transaction))
                            {
                                getPasswordCommand.CommandType = CommandType.StoredProcedure;
                                getPasswordCommand.Parameters.AddWithValue("@idUser", idUser);
                                currentPassword = (string?)await getPasswordCommand.ExecuteScalarAsync();

                                if (string.IsNullOrEmpty(currentPassword))
                                {
                                    transaction.Rollback();
                                    return "Mật khẩu không tồn tại";
                                }
                            }
                            if(!BCrypt.Net.BCrypt.Verify(password.OldPassword, currentPassword))
                            {
                                transaction.Rollback();
                                return "Mật khẩu không đúng";
                            }
                            string newHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                            using (var changePasswordCommand = new SqlCommand("ChangePassword", connection, transaction))
                            {
                                changePasswordCommand.CommandType = CommandType.StoredProcedure;
                                changePasswordCommand.Parameters.AddWithValue("@idUser", idUser);
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
                throw new Exception (e.Message, e);
            }
        }
    }
}
