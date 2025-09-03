using System;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{

    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "New";

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; } = 1;



        // Цена на момент заказа (фиксируется при добавлении товара)
        public decimal UnitPrice { get; set; }
    }
}
