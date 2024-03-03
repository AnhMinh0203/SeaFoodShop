using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.Models;
using SeaFoodShop.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SeaFoodShop.Repository
{
    public class SeaFoodRespon: ISeaFoodRespon
    {
        private readonly ConnectToSql _context;
        public SeaFoodRespon(ConnectToSql context)
        {
            _context = context;
        }
        /*
        public async Task<SeaFoodDetailModel> getSeaFoodDetailAsync (int id)
        {
            SeaFoodDetailModel seaFoodDetailModel = new SeaFoodDetailModel();
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection()){
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetSeaFoodDetail",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id",id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                seaFoodDetailModel.Id = reader.GetInt32(0);
                                seaFoodDetailModel.Name = reader.GetString(2);
                                seaFoodDetailModel.Price = reader.GetDecimal(3);
                                seaFoodDetailModel.Unit = reader.GetString(4);
                                seaFoodDetailModel.Description = reader.GetString(6);
                                seaFoodDetailModel.Status = reader.GetString(7);
                                seaFoodDetailModel.Instruct = reader.GetString(8);
                                seaFoodDetailModel.ExpirationDate = reader.GetString(9);
                                seaFoodDetailModel.Origin = reader.GetString(10);
                                seaFoodDetailModel.NameType = reader.GetString(11);
                        }
                    }
                }
                return seaFoodDetailModel;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
         */
        public async Task<List<SeaFoodModel>> getSeaFoodsAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();

                    var seaFoodList = await connection.QueryAsync<SeaFoodModel>(
                        "GetSeaFoods",
                        new { PageNumber = pageNumber, PageSize = pageSize },
                        commandType: CommandType.StoredProcedure);

                    return seaFoodList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching sea foods: {ex.Message}");
            }
        }

        public async Task<SeaFoodDetailModel> getSeaFoodDetailAsync (int id)
        {
            try
            {
                using (var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);
                    SeaFoodDetailModel? seaFoodDetailModel = await connection.QueryFirstOrDefaultAsync<SeaFoodDetailModel>(
                        "GetSeaFoodDetail",
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 5
                    );
                    return seaFoodDetailModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SeaFoodModel>> searchSeaFoodAsync (string nameSeaFood)
        {
            try
            {
                List<SeaFoodModel> seaFoodList = new List<SeaFoodModel>();
               
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    using(var command = new SqlCommand("SearchSeaFood", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@nameSeaFood", nameSeaFood);
                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                SeaFoodModel seaFood = new SeaFoodModel();
                                seaFood.Id = reader.GetInt32(0);
                                seaFood.Name = reader.GetString(2);
                                seaFood.Price = reader.GetDecimal(3);
                                seaFood.Unit = reader.GetString(4);
                                seaFoodList.Add(seaFood);
                            }
                        }
                    }
                }
                return seaFoodList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<SeaFoodModel>> searchSeaFoodByTypeAsync (string nameType)
        {
            try
            {
                using(var connection = (SqlConnection)_context.CreateConnection())
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@NameType", nameType);
                    var result = await connection.QueryAsync<SeaFoodModel>(
                        "SearchSeaFoodByType",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result.ToList();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }
    }
}
