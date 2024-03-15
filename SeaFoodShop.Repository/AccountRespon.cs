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

        public async Task<string> SignUpAsync(SignUpModel model)
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
    }
}
