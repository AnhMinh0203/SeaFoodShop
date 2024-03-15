using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository
{
    public class TypeRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public TypeRespon (ConnectToSql context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        public async Task<string> addTypeSeaFoodAsync (TypeModel type,string token)
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
                    var parameters = new DynamicParameters();
                    parameters.Add("@nameType", type.NameType);
                    parameters.Add("@result",dbType:DbType.String , direction: ParameterDirection.Output, size: 100);
                    
                    await connection.ExecuteAsync(
                        "AddSeaFoodType",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                    string result = parameters.Get<string>("@result");

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task <string> deleteTypeSeaFoodAsync (string nameType, string token)
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
                    var parameters = new DynamicParameters();
                    parameters.Add("@nameType", nameType);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "DeleteSeaFoodType",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure);
                    string result = parameters.Get<string>("@result");

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TypeModel>> getSeaFoodTypeAsync(string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                throw new Exception("Vui lòng đăng nhập");
            }
            try
            { 
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    return (await connection.QueryAsync<TypeModel>("GetSeaFoodType", commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
