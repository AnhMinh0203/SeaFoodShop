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
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace SeaFoodShop.Repository
{
    public class AddressRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public AddressRespon(ConnectToSql context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task <string> addAddressAsync (string token, AddressModel address)
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
                    var idAddress = Guid.NewGuid();

                    var parameters = new DynamicParameters();
                    parameters.Add("@idUser", idUser);
                    parameters.Add("@idAddress", idAddress);
                    parameters.Add("@nameAddress", address.NameAddress);
                    parameters.Add("@phoneNumber", address.PhoneNumber);
                    parameters.Add("@isDefault", address.isDefault);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "AddAddress",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var result = parameters.Get<string>( "result" );
                    return result;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AddressModel>?> getAddressAsync (string token)
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
                    var addressList = await connection.QueryAsync<AddressModel>(
                        "GetAddresses",
                        new { idUser = idUser},
                        commandType: CommandType.StoredProcedure);

                    return addressList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> deleteAddressAsync (string token, string idAddress)
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
                    var addressList = await connection.QueryAsync<AddressModel>(
                        "DeteleAddress",
                        new { idUser = idUser , idAddress = idAddress },
                        commandType: CommandType.StoredProcedure);

                    return "Xóa địa chỉ thành công";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
