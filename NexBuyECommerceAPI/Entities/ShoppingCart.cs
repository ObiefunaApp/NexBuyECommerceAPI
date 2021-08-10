using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Entities
{
    public class ShoppingCart
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public List<ShoppingCartItem> ProductCartItems { get; set; }

        public CartStatus CartStatus { get; set; } = CartStatus.ACTIVE;
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
