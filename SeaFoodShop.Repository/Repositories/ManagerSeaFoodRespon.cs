using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace SeaFoodShop.Repository.Repositories
{
    public class ManagerSeaFoodRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public ManagerSeaFoodRespon(ConnectToSql context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<string> addSeaFoodAsync (SeaFoodDetailModel seaFoodDetail,string token)
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
                    /* var seaFoodImagesJsonArray = seaFoodDetail.SeaFoodImages.Select(image => $"{{\"Image\": \"{image.nameImage}\"}}");
                     var jsonSeaFoodImages = $"[{string.Join(", ", seaFoodImagesJsonArray)}]";

                     var descriptionImagesJsonArray = seaFoodDetail.DescriptionImages.Select(image => $"{{\"Image\": \"{image.nameImage}\"}}");
                     var jsonDescriptionImages = $"[{string.Join(", ", descriptionImagesJsonArray)}]";*/

                    if (seaFoodDetail.DescriptionImages != null && seaFoodDetail.DescriptionImages.Count > 0)
                    {
                        seaFoodDetail.DescriptionImagesJson = JsonConvert.SerializeObject(seaFoodDetail.DescriptionImages);
                        var test = seaFoodDetail.DescriptionImagesJson;
                    }

                    if (seaFoodDetail.SeaFoodImages != null && seaFoodDetail.SeaFoodImages.Count > 0)
                    {
                        seaFoodDetail.SeaFoodImagesJson = JsonConvert.SerializeObject(seaFoodDetail.SeaFoodImages);
                        var test1 = seaFoodDetail.SeaFoodImagesJson;
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@name", seaFoodDetail.Name);
                    parameters.Add("@price", seaFoodDetail.Price);
                    parameters.Add("@unit", seaFoodDetail.Unit);
                    parameters.Add("@nameType", seaFoodDetail.TypeName);
                    parameters.Add("@status", seaFoodDetail.Status);
                    parameters.Add("@idVoucher", seaFoodDetail.IdVourcher);
                    parameters.Add("@instruct", seaFoodDetail.Instruct);
                    parameters.Add("@expirationDate", seaFoodDetail.ExpirationDate);
                    parameters.Add("@origin", seaFoodDetail.Origin);
                    parameters.Add("@primaryImage", seaFoodDetail.PrimaryImage);
                    parameters.Add("@jsonImagesSeaFood", seaFoodDetail.SeaFoodImagesJson);
                    parameters.Add("@jsonImagesDescription", seaFoodDetail.DescriptionImagesJson);
                    parameters.Add("@description", seaFoodDetail.Description);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "addSeaFood",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var result = parameters.Get<string>("result");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ("Error : " +  ex.Message);
            }
        } 

        public async Task<string> updateSeaFoodAsync (SeaFoodDetailModel seaFoodDetail, string token)
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
                    var seaFoodImagesJsonArray = seaFoodDetail.SeaFoodImages.Select(image => $"{{\"Image\": \"{image.nameImage}\"}}");
                    var jsonSeaFoodImages = $"[{string.Join(", ", seaFoodImagesJsonArray)}]";

                    var descriptionImagesJsonArray = seaFoodDetail.DescriptionImages.Select(image => $"{{\"Image\": \"{image.nameImage}\"}}");
                    var jsonDescriptionImages = $"[{string.Join(", ", descriptionImagesJsonArray)}]";

                    var parameters = new DynamicParameters();
                    parameters.Add("@idSeaFood", seaFoodDetail.Id);
                    parameters.Add("@name", seaFoodDetail.Name);
                    parameters.Add("@price", seaFoodDetail.Price);
                    parameters.Add("@unit", seaFoodDetail.Unit);
                    parameters.Add("@nameType", seaFoodDetail.TypeName);
                    parameters.Add("@status", seaFoodDetail.Status);
                    parameters.Add("@idVoucher", seaFoodDetail.IdVourcher);
                    parameters.Add("@instruct", seaFoodDetail.Instruct);
                    parameters.Add("@expirationDate", seaFoodDetail.ExpirationDate);
                    parameters.Add("@origin", seaFoodDetail.Origin);
                    parameters.Add("@primaryImage", seaFoodDetail.PrimaryImage);
                    parameters.Add("@jsonImagesSeaFood", jsonSeaFoodImages);
                    parameters.Add("@jsonImagesDescription", jsonDescriptionImages);
                    parameters.Add("@description", seaFoodDetail.Description);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "updateSeaFood",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var result = parameters.Get<string>("result");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ("Error : " + ex.Message);
            }
        }
        public async Task<string> deleteSeaFoodAsync (string token, string seaFoodId)
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
                    var parameters = new DynamicParameters();
                    parameters.Add("@idSeaFood", seaFoodId);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "deleteSeaFood",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var result = parameters.Get<string>("result");
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ("Error : " + ex.Message);
            }
        }
    }
}
