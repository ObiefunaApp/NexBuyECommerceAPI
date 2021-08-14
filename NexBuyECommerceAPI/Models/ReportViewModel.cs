using NexBuyECommerceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Models
{
    public class ReportViewModel
    {

        public decimal TotalRevenueForReport { get; set; }

        public List<Product> ReportProducts { get; set; }

        public List<Order> Orders { get; set; }

    }
}
