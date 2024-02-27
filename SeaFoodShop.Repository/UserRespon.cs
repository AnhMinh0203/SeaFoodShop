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
    public class UserRespon: IUser
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;

        public UserRespon(ConnectToSql context, IConfiguration configuration)
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
                                    var role = reader.GetInt32(reader.GetOrdinal("Role"));
                                    var claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, model.PhoneNumber), // Example claim
                                        new Claim(ClaimTypes.Role, role.ToString()), // Example claim
                                    };
                                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                                    var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"],
                                                    claims, expires: DateTime.Now.AddMinutes(120), 
                                                    signingCredentials: credentials);
                                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
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
