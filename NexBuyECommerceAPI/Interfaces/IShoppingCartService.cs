using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IShoppingCartService
    {

        ShoppingCart CreateCart(string userId);
        ShoppingCart GetCart(string userId, CartStatus cartStatus);
        void AddToCart(Product Product, string userId, int totalAmountOfPrescribedProducts = 0, int amount = 1);
        void ClearCart(string cartId);
        List<ShoppingCartItem> GetProductCartItems(string userId, CartStatus cartStatus);
        int RemoveFromCart(Product Product, string userId);

        decimal GetProductCartSumTotal(string userId);
        int GetProductCartTotalCount(string userId);
        Product GetProductById(int id);
        Product GetProductByName(string Name);
        ShoppingCartItem GetProductCartItemById(int id);
        ShoppingCart RefreshCart(string userId);

    }
}
