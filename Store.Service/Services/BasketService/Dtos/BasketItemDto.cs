using Store.Repoistory.BasketRepository.Models;
using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.BasketService.Dtos
{
    public class BasketItemDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Must be Greater than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Quantity Must be Between 1 and 10")]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string TypeName { get; set; }
    }
}