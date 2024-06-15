using Dapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using SeaFoodShop.Repository.Interface;
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
    public class ManagerSeaFoodRespon : IManagerSeaFood
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;

        private string PathToServiceAccountKey = @"D:\Rac\seafood-425116-b1657c549db1.json";
        public ManagerSeaFoodRespon(ConnectToSql context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<string> AddSeaFoodAsync (SeaFoodDetailModel seaFoodDetail,string token)
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
                    if (seaFoodDetail.SeaFoodImages != null && seaFoodDetail.SeaFoodImages.Count > 0)
                    {
                        seaFoodDetail.SeaFoodImagesJson = JsonConvert.SerializeObject(seaFoodDetail.SeaFoodImages);
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("@name", seaFoodDetail.Name);
                    parameters.Add("@price", seaFoodDetail.Price);
                    parameters.Add("@unit", seaFoodDetail.Unit);
                    parameters.Add("@nameType", seaFoodDetail.NameType);
                    parameters.Add("@idVoucher", seaFoodDetail.IdVourcher);
                    parameters.Add("@instruct", seaFoodDetail.Instruct);
                    parameters.Add("@expirationDate", seaFoodDetail.ExpirationDate);
                    parameters.Add("@origin", seaFoodDetail.Origin);
                    parameters.Add("@quantity", seaFoodDetail.Quantity);
                    parameters.Add("@primaryImage", seaFoodDetail.PrimaryImage);
                    parameters.Add("@jsonImagesSeaFood", seaFoodDetail.SeaFoodImagesJson);
                    parameters.Add("@description", seaFoodDetail.Description);
                    parameters.Add("@createBy", idUser);
                    parameters.Add("@modifyBy", idUser);
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

        public async Task<string> UpdateSeaFoodAsync (SeaFoodDetailModel seaFoodDetail,int idSeafood, string token)
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
                    if (seaFoodDetail.SeaFoodImages != null && seaFoodDetail.SeaFoodImages.Count > 0)
                    {
                        seaFoodDetail.SeaFoodImagesJson = JsonConvert.SerializeObject(seaFoodDetail.SeaFoodImages);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@idSeaFood", idSeafood);
                    parameters.Add("@name", seaFoodDetail.Name);
                    parameters.Add("@price", seaFoodDetail.Price);
                    parameters.Add("@unit", seaFoodDetail.Unit);
                    parameters.Add("@nameType", seaFoodDetail.NameType);
                    parameters.Add("@Quantity", seaFoodDetail.Quantity);
                    parameters.Add("@idVoucher", seaFoodDetail.IdVourcher);
                    parameters.Add("@instruct", seaFoodDetail.Instruct);
                    parameters.Add("@expirationDate", seaFoodDetail.ExpirationDate);
                    parameters.Add("@origin", seaFoodDetail.Origin);
                    parameters.Add("@primaryImage", seaFoodDetail.PrimaryImage);
                    parameters.Add("@jsonImagesSeaFood", seaFoodDetail.SeaFoodImagesJson);
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

        public async Task<string> DeleteSeaFoodAsync(string token, string seaFoodId)
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

        public async Task<UploadImageDriveModel> UploadImgProduct(IFormFile file)
        {
            try
            {
                //string credentialsPath = @"C:\Users\nguye\OneDrive\Máy tính\MinhLe\BE\BE\SeaFoodShop\SeaFoodShop\credentials.json";
                string currentDirectory = Directory.GetCurrentDirectory();
                string credentialsPath = Path.Combine(currentDirectory, "credentials.json");
                string folderId = "1nSplZRyY7PZB1617yBC3ejoaeS3ajS3d";
                var FileId = await UpdateFileToGoogledrive(credentialsPath, folderId, file);
                UploadImageDriveModel uploadImageDriveModel = new UploadImageDriveModel();
                uploadImageDriveModel.Id = FileId;
                return uploadImageDriveModel;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is FileNotFoundException)
                {
                    Console.WriteLine("File not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        static async Task<string> UpdateFileToGoogledrive(string credentialsPath, string folderId, IFormFile file)
        {
            var FileId = "";

            GoogleCredential googleCredential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(stream)
                .CreateScoped(new[]{DriveService.ScopeConstants.DriveFile});
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = "SeaFood"
                });

                var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = file.FileName,
                    Parents = new List<string> { folderId }
                };

                FilesResource.CreateMediaUpload request;
                using (var fileStream = file.OpenReadStream())
                {
                    request = service.Files.Create(fileMetaData, fileStream, file.ContentType);
                    request.Fields = "id";
                    await request.UploadAsync();
                }

                var fileRespon = request.ResponseBody;
                FileId = fileRespon.Id;
            }
            return FileId;
        }
    }
}
