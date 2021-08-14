using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Entities
{
    public class Report
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProductSales { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public decimal TotalRevenueForReport { get; set; }

        public List<Product> ReportProducts { get; set; }

        public List<Order> Orders { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
