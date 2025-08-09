using System;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal GetDiscountedPrice()
        {
            if ((DateTime.UtcNow - CreatedAt).TotalDays > 15)
            {
                return Price * 0.9m;
            }
            return Price;
        }
    }
}
