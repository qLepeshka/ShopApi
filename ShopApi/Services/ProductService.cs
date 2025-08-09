using ShopApi.Data;
using ShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public Product CreateProduct(Product product)
        {
            if (_context.Products.Any(p => p.Name == product.Name))
            {
                throw new InvalidOperationException("Товар с таким именем уже существует");
            }

            if (product.CategoryId.HasValue && product.CategoryId != 1)
            {
                var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == product.CategoryId);
                if (category != null && category.Products.Count >= Category.MaxProductsPerCategory)
                {
                    throw new InvalidOperationException($"Категория не может содержать более {Category.MaxProductsPerCategory} товаров");
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product UpdateProduct(int id, Product product)
        {
            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null)
            {
                throw new ArgumentException("Товар не найден");
            }

            if (_context.Products.Any(p => p.Name == product.Name && p.Id != id))
            {
                throw new InvalidOperationException("Товар с таким именем уже существует");
            }

            if (product.CategoryId.HasValue && product.CategoryId != existingProduct.CategoryId && product.CategoryId != 1)
            {
                var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == product.CategoryId);
                if (category != null && category.Products.Count >= Category.MaxProductsPerCategory)
                {
                    throw new InvalidOperationException($"Категория не может содержать более {Category.MaxProductsPerCategory} товаров");
                }
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            _context.SaveChanges();
            return existingProduct;
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                throw new ArgumentException("Товар не найден");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToList();
        }
    }
}
