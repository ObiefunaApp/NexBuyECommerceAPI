using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Transactions;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface ITransactionService
    {

        Task<Transaction> CreateTransaction(string txRef, string generatedReference, int orderId);
        Task<Transaction> GetTransactionByTxRef(string txRef);
        Task<Transaction> GetTransactionByGeneratedRef(string generatedRef);
        Task<Transaction> GetTransactionById(int id);
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetTransactionsByStatus(TransactionStatus transactionStatus);

        Task<Transaction> GetLastTransaction();

    }
}
