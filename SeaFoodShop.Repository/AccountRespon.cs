using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using System.Data;
using System.Text.RegularExpressions;
using BCrypt.Net;
using SeaFoodShop.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dapper;

namespace SeaFoodShop.Repository
{
    public class AccountRespon: IAccountRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;

        public AccountRespon(ConnectToSql context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        public async Task<CustomMessage> SignInAsync (SignInModel model)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using(var command = new SqlCommand("SignIn", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@phoneNumber", model.PhoneNumber);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if(reader.Read())
                            {
                                int statusAccount = reader.GetInt32(reader.GetOrdinal("Status"));
                                if(statusAccount == -1)
                                {
                                    return new CustomMessage
                                    {
                                        Message = "Tài khoản đã bị khóa. Vui lòng liên hệ Admin",
                                        Token = null
                                    };
                                }
                                string hashedPassword = reader.GetString(reader.GetOrdinal("Password"));
                                // Verify the input password against the hashed password using bcrypt
                                bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, hashedPassword);
                                if (passwordMatch)
                                {
                                    var idUser = reader.GetGuid(reader.GetOrdinal("Id"));
                                    var tokenRespon = new TokenRespon(_config);
                                    var token = tokenRespon.GenerateJwtToken(model, idUser);
                                    return new CustomMessage
                                    {
                                        Message = "Login Successfully",
                                        Token = token
                                    };
                                }
                            }
                            return new CustomMessage
                            {
                                Message = "Phone number or password is not correct",
                                Token = null
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return new CustomMessage
                {
                    Message = $"SQL Error: {ex.Message}",
                    Token = null
                };
            }
        }

        public async Task<string?> SignUpAsync(SignUpModel model)
        {
            try
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Generate a salt for bcrypt encryption
                string hash = BCrypt.Net.BCrypt.HashPassword(model.Password, salt); // Hash the password with bcrypt
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using(var command = new SqlCommand("SignUp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@fullName", model.FullName);
                        command.Parameters.AddWithValue ("@password", hash);
                        command.Parameters.AddWithValue("@phoneNumber", model.PhoneNumber);
                        command.Parameters.AddWithValue("@dob", model.Dob);

                        // Add an output parameter to get the result from the stored procedure
                        var resultParam = new SqlParameter("@result",SqlDbType.NVarChar, 100); 
                        resultParam.Direction = ParameterDirection.Output; 
                        command.Parameters.Add(resultParam);

                        await command.ExecuteNonQueryAsync();
                        return resultParam.Value.ToString();
                    }
                }
            }
            catch (SecurityTokenException ex)
            {
                return $"Invalid token: {ex.Message}";
            }
            catch (SqlException ex)
            {
                return $"SQL Error: {ex.Message}";
            }
        }

        public async Task<string> ForgetPassword(string phoneNumber)
        {
            return "Your new password";
        }

        public async Task<string> LockAccountAsync (string phoneNumber)
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
        public async Task<string> UnlockAccountAsync (string phoneNumber)
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

        public async Task<List<AccountModel>?> SearchAccountAsync (string token,string phoneNumber,string status)
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
                        new { phoneNumber = phoneNumber , status = status},
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
                    using (var transaction = connection.BeginTransaction())
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
                            if (!BCrypt.Net.BCrypt.Verify(password.OldPassword, currentPassword))
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
                throw new Exception(e.Message, e);
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
