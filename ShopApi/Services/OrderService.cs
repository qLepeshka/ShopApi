using ShopApi.Data;
using ShopApi.Models;

using Microsoft.EntityFrameworkCore;

namespace ShopApi.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
        }

        public Order? GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);
        }

        public Order CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order UpdateOrder(int id, Order order)
        {
            var existingOrder = _context.Orders.Find(id);
            if (existingOrder == null)
            {
                throw new ArgumentException("Заказ не найден");
            }

            if (order.Status == "Processing" && existingOrder.Status == "Delivered")
            {
                throw new InvalidOperationException("Нельзя изменить статус доставленного заказа на 'Processing'");
            }

            existingOrder.CustomerName = order.CustomerName;
            existingOrder.Address = order.Address;
            existingOrder.Status = order.Status;

            _context.SaveChanges();
            return existingOrder;
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                throw new ArgumentException("Заказ не найден");
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetOrdersByStatus(string status)
        {
            return _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
        }

        public void AddProductToOrder(int orderId, int productId, int quantity = 1)
        {
            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                throw new ArgumentException("Заказ не найден");
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new ArgumentException("Товар не найден");
            }

            if (order.OrderItems.Any(oi => oi.ProductId == productId))
            {
                throw new InvalidOperationException("Товар уже добавлен в заказ");
            }

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.GetDiscountedPrice()
            };

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.OrderId == orderId && oi.ProductId == productId);
            if (orderItem == null)
            {
                throw new ArgumentException("Товар не найден в заказе");
            }

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
        }
    }
}
