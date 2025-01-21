using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.ModelsDto
{
    public class ProductModelDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string ImgUri { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}
