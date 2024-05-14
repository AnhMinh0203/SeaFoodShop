using SeaFoodShop.DataContext.Models;

namespace SeaFoodShop.Models
{

    public class SeaFoodModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? NameType { get; set; }
        public string? Unit { get; set; }
        public int? IdVourcher { get; set; }  
        public int RowNum { get; set; }
    }
    public class ListSeaFoodModel 
    {
        public List<SeaFoodModel>? ListSeaFood { get; set; }
        public int TotalRecord { get; set; }
    }
}
