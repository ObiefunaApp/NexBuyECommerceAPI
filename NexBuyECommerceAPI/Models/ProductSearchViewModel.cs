using NexBuyECommerceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Models
{
    public class ProductSearchViewModel
    {

        public string SearchString { get; set; }
        public List<Product> Products { get; set; }

    }
}
