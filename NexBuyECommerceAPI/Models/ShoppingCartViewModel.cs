using NexBuyECommerceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Models
{
    public class ShoppingCartViewModel
    {

        public List<ShoppingCartItem> CartItems { get; set; }

        public decimal ProductCartTotal { get; set; }
        public int ProductCartItemsTotal { get; set; }
        public int ProductId { get; set; }

    }
}
