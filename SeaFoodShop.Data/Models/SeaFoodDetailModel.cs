
using System.Text.Json.Serialization;

namespace SeaFoodShop.Models
{
    public class SeaFoodDetailModel: SeaFoodModel
    {
        public string? Description { get; set; }
        public string? Instruct {  get; set; }
        public string? ExpirationDate { get; set; }
        public string? Origin { get; set; }
        public string? TypeName { get; set; }
        public string? PrimaryImage { get; set; }
        [JsonIgnore]
        public string? DescriptionImagesJson { get; set; }
        [JsonIgnore]
        public string? SeaFoodImagesJson { get; set; }
        public List<ImageDescModel>? DescriptionImages { get; set; }
        public List<ImageSeaFoodModel>? SeaFoodImages { get; set; }
    }

    public class ImageDescModel
    {
        public string? nameImage { get; set; }
    }
    public class ImageSeaFoodModel
    {
        public string? nameImage { get; set; }
    }
}
