using NexBuyECommerceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IOrderService
    {

        Order CreateOrder(Order order, string userId, string clearedBy);

        Order GetOrderById(int orderId);

       // List<Order> GetOrdersForTheDay();

        List<Order> GetOrdersForTheWeek();

        List<Order> GetOrdersForTheMonth();

        decimal GetTotalRevenue();
        int GetTotalSales();

    }
}
