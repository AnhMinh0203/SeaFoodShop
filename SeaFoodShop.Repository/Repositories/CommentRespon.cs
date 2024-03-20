using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext;
using System.Data;
using System.Text.RegularExpressions;
using BCrypt.Net;
using SeaFoodShop.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Twilio.TwiML.Voice;
using Twilio.Jwt.AccessToken;
using SeaFoodShop.Models;
using Newtonsoft.Json;

namespace SeaFoodShop.Repository.Repositories
{
    public class CommentRespon
    {
        private readonly ConnectToSql _context;
        private readonly IConfiguration _config;
        public CommentRespon(ConnectToSql context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        public async Task<bool?> postCommentAsync(CommentModel comment, string token)
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
                    using (var command = new SqlCommand("PostComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var imageTable = new DataTable();
                        imageTable.Columns.Add("Image", typeof(string));
                        foreach (var image in comment.Images)
                        {
                            imageTable.Rows.Add(image.nameImage);
                        }
                        command.Parameters.AddWithValue("@Images", imageTable);
                        command.Parameters.AddWithValue("@CommentText", comment.TextComment);
                        command.Parameters.AddWithValue("@Like", comment.LikeCount);
                        command.Parameters.AddWithValue("@Dislike", comment.Dislike);
                        command.Parameters.AddWithValue("@IdFood", comment.IdFood);
                        command.Parameters.AddWithValue("@Stars", comment.Star);
                        command.Parameters.AddWithValue("@IdUser", idUser.ToString());
                        await command.ExecuteNonQueryAsync();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool?> updateCommentAsync(CommentUpdateModel commentUpdate, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
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
                    using (var command = new SqlCommand("UpdateComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CommentId", commentUpdate.Id);
                        command.Parameters.AddWithValue("@CommentText", commentUpdate.TextComment);
                        command.Parameters.AddWithValue("@Stars", commentUpdate.Star);

                        DataTable imagesTable = new DataTable();
                        imagesTable.Columns.Add("Image", typeof(string));

                        foreach (var image in commentUpdate.Images)
                        {
                            imagesTable.Rows.Add(image.nameImage);
                        }

                        SqlParameter imagesParameter = new SqlParameter("@Images", SqlDbType.Structured);
                        imagesParameter.Value = imagesTable;
                        imagesParameter.TypeName = "dbo.ImageTableType";
                        command.Parameters.Add(imagesParameter);
                        await command.ExecuteNonQueryAsync();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool?> deleteCommentAsync(string id, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
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
                    using (var command = new SqlCommand("DeleteComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        await command.ExecuteNonQueryAsync();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // C1
        /*public async Task<List<CommentSearchModel>?> searchCommentAsync(string text, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return null;
            }
            try
            {
                List<CommentSearchModel> results = new List<CommentSearchModel>();

                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using var command = new SqlCommand("SearchComments", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentText", text);
                    using SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        string commentText = reader.GetString(reader.GetOrdinal("Comment"));
                        string imageName = reader.GetString(reader.GetOrdinal("Image"));

                        // Check if the comment already exists in the results list
                        var existingComment = results.FirstOrDefault(c => c.TextComment == commentText);

                        if (existingComment != null)
                        {
                            // Comment already exists, add the image to its images list
                            existingComment.Images.Add(new ImageModel { nameImage = imageName });
                        }
                        else
                        {
                            // Comment doesn't exist, create a new CommentSearchModel and add it to the results list
                            var newComment = new CommentSearchModel
                            {
                                TextComment = commentText,
                                Images = new List<ImageModel> { new ImageModel { nameImage = imageName } }
                            };
                            results.Add(newComment);
                        }
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/

        //C2

        public async Task<List<CommentSearchModel>?> searchCommentAsync(string text, string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null) return null;
            try
            {
                List<CommentSearchModel> results = new List<CommentSearchModel>();
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using var command = new SqlCommand("SearchComments", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentText", text);
                    using SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        CommentSearchModel commentSearch = new CommentSearchModel
                        {
                            TextComment = reader.GetString(reader.GetOrdinal("Comment")),
                            Images = new List<ImageModel>()
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Pictures")))
                        {
                            var imagesJson = reader.GetString(reader.GetOrdinal("Pictures"));
                            // Convert imageJson string to list object ImageModel
                            var images = JsonConvert.DeserializeObject<List<ImageModel>>(imagesJson);
                            commentSearch.Images.AddRange(images);
                        }
                        results.Add(commentSearch);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
