using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Entities
{
    public class ShoppingCartItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Amount { get; set; }
        public int PrescribedAmount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductCartId { get; set; }

        public ProductCart ProductCart { get; set; }

    }
}
