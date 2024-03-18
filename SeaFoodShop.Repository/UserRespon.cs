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
    }
}
