using Dapper;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;
using Twilio.TwiML.Voice;


namespace SeaFoodShop.Repository.Repositories
{
    public class ManagerBlogRespon
    {
        private readonly IConfiguration _config;
        private readonly ConnectToSql _context;
        public ManagerBlogRespon (ConnectToSql connectToSql,IConfiguration config)
        {
            _config = config;
            _context = connectToSql;
        }

        public async Task<string> UploadImgBlogAsync(IFormFile file, string idFolder)
        {
            CommonFunction commonFunction = new();
            var resultUploadImg = await commonFunction.UploadImg(file, idFolder);
            return resultUploadImg.Id;
        } 
        public async Task<string> UploadContentBlogAsync(BlogDetailModel blog,string token)
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
                    parameters.Add("@IdUser", idUser);
                    parameters.Add("@Title", blog.Title);
                    parameters.Add("@Content", blog.Content);
                    parameters.Add("@Thumbnail", blog.Thumbnail);
                    parameters.Add("@View", blog.Views);
                    parameters.Add("@Like", blog.Likes);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "AddBlogContent",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var resultUploadContent = parameters.Get<string>("result");
                    return resultUploadContent;
                }
            }
            catch (Exception ex)
            {
                return ("Error : " + ex.Message);
            }
        }

        public async Task<string?> DeleteImgBlogAsync(string idImg)
        {
            CommonFunction commonFunction = new();
            var resultUploadImg = await commonFunction.DeleteImg(idImg);
            return resultUploadImg;
        }
        
        public async Task<string?> UpdateImgBlogAsync(IFormFile newFile, string idFolder, string idImg)
        {
            CommonFunction commonFunction = new();
            var resultUpdateImg = await commonFunction.UpdateImg(idImg, newFile, idFolder);
            return resultUpdateImg.Id;
        }

        public async Task<string?> UpdateBlogContentAsync(BlogUpdateModel blog, string token, string idBlog)
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
                    parameters.Add("@IdBlog", idBlog);
                    parameters.Add("@Title", blog.Title);
                    parameters.Add("@Content", blog.Content);
                    parameters.Add("@Thumbnail", blog.Thumbnail);
                    parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "UpdateBlogContent",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var resultUploadContent = parameters.Get<string>("result");
                    return resultUploadContent;
                }
            }
            catch (Exception ex)
            {
                return ("Error : " + ex.Message);
            }
        }

        /*public async Task<BlogDetailModel?> GetDetailBlogAsync(string idBlog)
        {
            using(var connection = (SqlConnection)_context.CreateConnection())
            {
                await connection.OpenAsync();
                var parameters = new { Id = idBlog };
                var result = await connection.QueryFirstOrDefaultAsync<BlogDetailModel>(
                        "GetBlogDetail",
                        parameters,
                        commandType : CommandType.StoredProcedure);
                return result;
            }
        }  */
    }
}
