using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//using System.Transactions;

namespace NexBuyECommerceAPI.Entities
{
    public class Transaction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public string GeneratedReferenceNumber { get; set; }
        public TransactionStatus TransactionStatus { get; set; } = TransactionStatus.PENDING;
        public DateTime CreatedAt { get; } = DateTime.Now;
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
