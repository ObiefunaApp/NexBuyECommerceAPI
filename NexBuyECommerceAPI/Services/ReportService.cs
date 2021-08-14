using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.ExtensionMethods;
using NexBuyECommerceAPI.Interfaces;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Services
{
    public class ReportService : IReportService
    {

        private readonly ApplicationContext _dbContext;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _ShoppingCartService;
        private readonly IProductService _ProductService;

        public ReportService(IOrderService orderService, IShoppingCartService ShoppingCartService, IProductService ProductService, ApplicationContext dbContext)
        {
            _orderService = orderService;
            _ShoppingCartService = ShoppingCartService;
            _ProductService = ProductService;
            _dbContext = dbContext;
        }


        public Report CreateReport(TimeFrame timeFrame)
        {
            Report report;
            switch (timeFrame)
            {
                //case TimeFrame.DAILY:
                //    {
                //        Func<Report, bool> dailyFunc = report1 => report1.CreatedAt.Date == DateTime.Now.Date && report1.TimeFrame == timeFrame;
                //        report = GetReportByFunc(dailyFunc) ?? new Report();
                //        var orders = _orderService.GetOrdersForTheDay();
                //        report.Orders = orders;
                //        report.TimeFrame = timeFrame;
                //        report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                //        var ProductItem = new List<ShoppingCartItem>();
                //        var Products = new List<Product>();

                //        foreach (var order in orders)
                //        {
                //            foreach (var ProductCartItem in order.OrderItems)
                //            {
                //                ProductItem.Add(_ShoppingCartService.GetProductCartItemById(ProductCartItem.Id));
                //                Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                //            }
                //        }

                //        report.ProductSales = GenerateSalesTable(ProductItem);
                //        report.ReportProducts = Products;

                //        if (GetReportBoolByFunc(dailyFunc))
                //        {
                //            _dbContext.Entry(report).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
                //        }
                //        else
                //        {
                //            _dbContext.Reports.Add(report);
                //        }

                //        _dbContext.SaveChanges();
                //        return report;
                //    }
                case TimeFrame.WEEKLY:
                    {
                        var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
                        var lastDayOfWeek = DateTime.Now.LastDayOfWeek();
                        Func<Report, bool> weeklyFunc = report1 =>
                            report1.CreatedAt.Month.Equals(beginningOfWeek.Month) &&
                            report1.CreatedAt.Year.Equals(beginningOfWeek.Year) && (report1.CreatedAt >= beginningOfWeek &&
                            report1.CreatedAt <= lastDayOfWeek && report1.TimeFrame == timeFrame);


                        report = GetReportByFunc(weeklyFunc) ?? new Report();

                        var orders = _orderService.GetOrdersForTheWeek();
                        report.Orders = orders;
                        report.TimeFrame = timeFrame;
                        report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                        var ProductItem = new List<ShoppingCartItem>();
                        var Products = new List<Product>();

                        foreach (var order in orders)
                        {
                            foreach (var ProductCartItem in order.OrderItems)
                            {
                                ProductItem.Add(_ShoppingCartService.GetProductCartItemById(ProductCartItem.Id));
                                Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                            }
                        }

                        report.ReportProducts = Products;
                        report.ProductSales = GenerateSalesTable(ProductItem);

                        if (GetReportBoolByFunc(weeklyFunc))
                        {
                            _dbContext.Entry(report).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
                        }
                        else
                        {
                            _dbContext.Reports.Add(report);
                        }

                        _dbContext.SaveChanges();
                        return report;
                    }
                case TimeFrame.MONTHLY:
                    {
                        Func<Report, bool> monthlyFunc = report1 =>
                            report1.CreatedAt.Month.Equals(DateTime.Now.Month) &&
                            report1.CreatedAt.Year.Equals(DateTime.Now.Year) && report1.TimeFrame == timeFrame;

                        report = GetReportByFunc(monthlyFunc);
                        if (report == null)
                        {
                            report = new Report();
                        }

                        var orders = _orderService.GetOrdersForTheMonth();
                        report.Orders = _orderService.GetOrdersForTheMonth();
                        report.TimeFrame = timeFrame;
                        report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                        var ProductItem = new List<ShoppingCartItem>();
                        var Products = new List<Product>();

                        foreach (var order in orders)
                        {
                            foreach (var ProductCartItem in order.OrderItems)
                            {
                                ProductItem.Add(_ShoppingCartService.GetProductCartItemById(ProductCartItem.Id));
                                Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                            }
                        }

                        report.ProductSales = GenerateSalesTable(ProductItem);
                        report.ReportProducts = Products;
                        if (GetReportBoolByFunc(monthlyFunc))
                        {
                            _dbContext.Entry(report).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
                        }
                        else
                        {
                            _dbContext.Reports.Add(report);
                        }

                        _dbContext.SaveChanges();
                        return report;
                    }
                default: return null;
            }
        }
        public bool GetReportBoolByFunc(Func<Report, bool> func)
        {
            return _dbContext.Reports.Any(func);
        }

        public Report GetReportByFunc(Func<Report, bool> func)
        {
            return _dbContext.Reports.FirstOrDefault(func);
        }


        private static string GenerateSalesTable(List<ShoppingCartItem> cartItems)
        {
            var sb = new StringBuilder();
            const string table = @"
                                <table class= "" table table-hover table-bordered text-left "">
                                <thead>
                                    <tr class= ""table-success "">
                                    <th>Product Name</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    </tr>
                                </thead>";
            sb.Append(table);
            foreach (var item in cartItems)
            {
                var row = $@"<tbody>
                                <tr class=""info"" style="" cursor: pointer"">
                                <td class=""font-weight-bold"">{item.Product.ProductName}</td>
                                <td class=""font-weight-bold"">{item.Amount}</td>
                                <td class=""font-weight-bold"">{item.Product.Price}</td>
                         </tr>
                         </tbody>";
                sb.Append(row);
            }

            sb.Append("</Table>");
            return sb.ToString();
        }

    }
}
