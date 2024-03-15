namespace SeaFoodShop.Models
{

    public class SeaFoodModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public int IdVourcher { get; set; }  
    }
}
