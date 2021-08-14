using Microsoft.EntityFrameworkCore;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.ExtensionMethods;
using NexBuyECommerceAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _dbContext;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(ApplicationContext dbContext, IShoppingCartService shoppingCartService)
        {
            _dbContext = dbContext;
            _shoppingCartService = shoppingCartService;
        }

        public Order CreateOrder(Order order, string userId, string clearedBy)
        {
            var cart = _shoppingCartService.GetCart(userId, CartStatus.ACTIVE);
            order.OrderItems = cart.ProductCartItems;
            order.Price = _shoppingCartService.GetProductCartSumTotal(userId);
            order.ClearedBy = clearedBy;

            cart.ProductCartItems.ForEach(item =>
            {
                item.Product.Quantity -= item.Amount;
                _dbContext.Entry(item.Product).State = EntityState.Modified;
            });

            _dbContext.Orders.Add(order);
            cart.CartStatus = CartStatus.MOST_RECENT;
            _dbContext.Entry(cart).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return order;
        }

        public Order GetOrderById(int orderId)
        {
            return _dbContext.Orders.FirstOrDefault(order => order.OrderId == orderId);
        }

        //public List<Order> GetOrdersForTheDay()
        //{
        //    return _dbContext.Orders.Include(order => order.OrderItems).Where(order => DbFunctions.TruncateTime(order.CreatedAt) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        //}

        public List<Order> GetOrdersForTheMonth()
        {
            return _dbContext.Orders.Include(order => order.OrderItems).Where(order => order.CreatedAt.Month.Equals(DateTime.Now.Month) &&
                                              order.CreatedAt.Year.Equals(DateTime.Now.Year))
                .ToList();
        }

        public List<Order> GetOrdersForTheWeek()
        {
            var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
            var lastDayOfTheWeek = DateTime.Now.LastDayOfWeek();

            var orders = _dbContext.Orders.Include(order => order.OrderItems).Where(order => DateTime.Now.Month == order.CreatedAt.Month
                    && DateTime.Now.Year == order.CreatedAt.Year)
                .Where(order => order.CreatedAt >= beginningOfWeek && order.CreatedAt < lastDayOfTheWeek)
                .ToList();
            return orders;
        }

        public decimal GetTotalRevenue()
        {
            var totalSales = _dbContext.Orders.Select(order => order.OrderItems).Count();
            return totalSales;
        }

        public int GetTotalSales()
        {
            var totalSales = _dbContext.Orders.Select(order => order.OrderItems).Count();
            return totalSales;
        }
    }
}
