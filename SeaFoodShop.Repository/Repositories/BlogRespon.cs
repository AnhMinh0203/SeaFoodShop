using Dapper;
using Microsoft.Data.SqlClient;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Repositories
{
    public class BlogRespon
    {
        private readonly ConnectToSql _context;
        public BlogRespon(ConnectToSql context)
        {
            _context = context;
        }

        public async Task<List<BlogModel>?> getBlogsAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();

                    var blogList = await connection.QueryAsync<BlogModel>(
                        "getBlogs",
                        new { PageNumber = pageNumber, PageSize = pageSize },
                        commandType: CommandType.StoredProcedure);

                    return blogList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching sea foods: {ex.Message}");
            }
        }

        public async Task<BlogDetailModel?> getBlogDetailAsync(string idBlog)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@idBlog", idBlog);
                    var result = await connection.QuerySingleOrDefaultAsync<BlogDetailModel>("getBlogDetail", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
