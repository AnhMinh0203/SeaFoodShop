
namespace SeaFoodShop.Models
{
    public class SeaFoodDetailModel: SeaFoodModel
    {
        public string Description { get; set; }
        public string Status { get; set; }
        public string Instruct {  get; set; }
        public string ExpirationDate { get; set; }
        public string Origin { get; set; }
        public string NameType { get; set; }
    }
}
