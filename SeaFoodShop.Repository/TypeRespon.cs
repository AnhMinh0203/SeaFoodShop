using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
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

        public async Task<string?> addTypeSeaFoodAsync (TypeModel type,string token)
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
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    return "Thêm loại thành công";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
