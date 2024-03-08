using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace SeaFoodShop.Repository
{
    public class ShoppingCartRespon
    {

        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public ShoppingCartRespon (ConnectToSql context,IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<string> addToShoppingCart (ShoppingCartModel shoppingCartModel, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return "Vui lòng đăng nhập tài khoản";
            }
            try
            {
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using(var command = new SqlCommand("AddToShoppingCart", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdSeafood", shoppingCartModel.IdFood);
                        command.Parameters.AddWithValue("@IdUser", idUser);
                        command.Parameters.AddWithValue("@Quantity", shoppingCartModel.Quantity);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return "Thêm sản phẩm vào giỏ hàng thành công";
                        }
                        return "Sản phẩm hiện đã hết vui lòng quay lại sau";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);    
            }
        }
        public async Task<string> updateShoppingCartAsync (ShoppingCartModel shoppingCartModel, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return "Vui lòng đăng nhập tài khoản";
            }
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("UpdateShoppingCart", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdSeafood", shoppingCartModel.IdFood);
                        command.Parameters.AddWithValue("@IdUser", idUser);
                        command.Parameters.AddWithValue("@Quantity", shoppingCartModel.Quantity);

                        await command.ExecuteNonQueryAsync();
                    }
                    return "Cập nhật thành công";
                }      
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> deleteShoppingCartAsync (string idSeaFood, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return "Vui lòng đăng nhập tài khoản";
            }
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("DeleteShoppingCart", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdSeaFood", idSeaFood);
                        command.Parameters.AddWithValue("@IdUser", idUser);
                        await command.ExecuteNonQueryAsync();

                        return "Xóa sản phẩm thành công";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        } 

        public async Task<List<SeaFoodModel>?> searchShoppingCartAsync (string nameSeaFood, string token)
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
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdUser", idUser);
                    parameters.Add("@NameSeaFood", nameSeaFood);
                    var result = await connection.QueryAsync<SeaFoodModel>(
                        "SearchShoppingCart",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result.ToList(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
