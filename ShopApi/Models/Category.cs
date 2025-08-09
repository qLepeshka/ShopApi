﻿using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{

    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public static int MaxProductsPerCategory { get; set; } = 100;
    }
}
