using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<>;

        public ShoppingCartService(ApplicationContext context)
        {
            _dbContext = context;
        }

        public void AddToCart(Product Product, string userId, int totalAmountOfPrescribedProducts = 0, int amount = 1)
        {
            var ProductCart = GetCart(userId, CartStatus.ACTIVE);
            var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(item => item.ProductId == Product.Id && item.ProductCartId == ProductCart.Id);
            if (cartItem == null)
            {
                cartItem = new ShoppingCartItem
                {
                    ProductCartId = ProductCart.Id,
                    ProductCart = ProductCart,
                    Product = Product,
                    PrescribedAmount = totalAmountOfPrescribedProducts * Product.PricePerUnit,
                    Amount = amount
                };
                _dbContext.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                if (cartItem.Amount < cartItem.Product.Quantity)
                    cartItem.Amount++;
            }
            _dbContext.SaveChanges();
        }

        public void ClearCart(string userId)
        {
            var cart = GetCart(userId, CartStatus.ACTIVE);
            _dbContext.ShoppingCartItems.RemoveRange(cart.ProductCartItems);
            cart.ProductCartItems = new List<ShoppingCartItem>();
            _dbContext.Entry(cart).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public ShoppingCart CreateCart(string userId)
        {
            var user = UserManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);
            var cart = new ShoppingCart()
            {
                ApplicationUser = user,
                ApplicationUserId = userId,
                ProductCartItems = new List<ShoppingCartItem>(),
                CartStatus = CartStatus.ACTIVE
            };

            _dbContext.ShoppingCarts.Add(cart);
            _dbContext.SaveChanges();

            return cart;
        }

        public ShoppingCart GetCart(string userId, CartStatus cartStatus)
        {
            var user = userManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);

            if (user != null)
                return _dbContext.ShoppingCarts.Include(cart => cart.ProductCartItems)
                    .FirstOrDefault(cart => cart.ApplicationUserId == userId && cart.CartStatus == cartStatus);

            return null;
        }

        public Product GetProductById(int id)
        {
            var Product = _dbContext.Products.FirstOrDefault(x => x.Id == Id);
            return Product;
        }

        public Product GetProductByName(string Name)
        {
            var Product = _dbContext.Products.FirstOrDefault(x => x.ProductName == Name);
            return Product;
        }

        public ShoppingCartItem GetProductCartItemById(int id)
        {
            var Product = _dbContext.ShoppingCartItems.Include(item => item.Product).FirstOrDefault(x => x.Id == id);
            return Product;
        }

        public List<ShoppingCartItem> GetProductCartItems(string userId, CartStatus cartStatus)
        {
            var cart = GetCart(userId, cartStatus);
            return _dbContext.ShoppingCartItems.Include(item => item.Product).Where(item => item.ProductCartId == cart.Id).ToList();
        }

        public decimal GetProductCartSumTotal(string userId)
        {
            var cart = GetCart(userId, CartStatus.ACTIVE);
            if (cart.ProductCartItems.Count == 0)
            {
                return 0;
            }
            var sum = _dbContext.ShoppingCartItems.Where(c => c.ProductCartId == cart.Id)
                .Select(c => (c.Product.Price * c.Amount) + c.PrescribedAmount).Sum();

            return sum;
        }

        public int GetProductCartTotalCount(string userId)
        {
            var cart = GetCart(userId, CartStatus.ACTIVE);
            var total = _dbContext.ShoppingCartItems.Count(c => c.ProductCartId == cart.Id); ;


            return total;
        }

        public ShoppingCart RefreshCart(string userId)
        {
            var ProductCart = GetCart(userId, CartStatus.MOST_RECENT);
            var newCart = CreateCart(userId);
            ProductCart.CartStatus = CartStatus.USED;
            _dbContext.Entry(ProductCart).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return newCart;
        }

        public int RemoveFromCart(Product Product, string userId)
        {
            var ProductCart = GetCart(userId, CartStatus.ACTIVE);
            var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(item => item.ProductId == Product.Id);
            if (cartItem != null)
            {
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    ProductCart.ProductCartItems.Remove(cartItem);
                    _dbContext.Entry(ProductCart).State = EntityState.Modified;
                    _dbContext.ShoppingCartItems.Remove(cartItem);
                }
            }
            _dbContext.SaveChanges();
            if (cartItem != null) return cartItem.Amount;
            return 0;
        }
    }
}
