using ShopApi.Data;
using ShopApi.Models;
using Microsoft.EntityFrameworkCore;


namespace ShopApi.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public Category CreateCategory(Category category)
        {
            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                throw new InvalidOperationException("Категория с таким именем уже существует");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category UpdateCategory(int id, Category category)
        {
            var existingCategory = _context.Categories.Find(id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Категория не найден");
            }

            if (_context.Categories.Any(c => c.Name == category.Name && c.Id != id))
            {
                throw new InvalidOperationException("Категория с таким именем уже существует");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _context.SaveChanges();
            return existingCategory;
        }

        public void DeleteCategory(int id, int? moveToCategoryId = null)
        {
            if (id == 1)
            {
                throw new InvalidOperationException("Нельзя удалить категорию 'Без категории'");
            }

            var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                throw new ArgumentException("Категория не найдена");
            }

            if (category.Products.Any())
            {
                if (moveToCategoryId.HasValue)
                {
                    foreach (var product in category.Products)
                    {
                        product.CategoryId = moveToCategoryId.Value;
                    }
                }
                else
                {
                    foreach (var product in category.Products)
                    {
                        product.CategoryId = 1;
                    }
                }
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
