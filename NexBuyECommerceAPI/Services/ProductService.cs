﻿using Microsoft.EntityFrameworkCore;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _dbContext;


        public ProductService(ApplicationContext context)
        {
            _dbContext = context;
        }

        public void AddProduct(Product Product)
        {
            _dbContext.Products.Add(Product);
            _dbContext.SaveChanges();
        }

        public void AddProductCategory(ProductCategory category)
        {
            _dbContext.ProductCategories.Add(category);
            _dbContext.SaveChanges();
        }

        public List<ProductCategory> AllCategories()
        {
            var res = _dbContext.ProductCategories.ToList();
            return res;
        }

        public int DateComparison(DateTime FirstDate, DateTime SecondDate)
            => DateTime.Compare(FirstDate, SecondDate);

        public Product EditProduct(int id)
        {
            var Product = GetProductById(id);
            if (Product == null)
                return null;

            return _dbContext.Products.SingleOrDefault(d => d.Id == id);
        }

        public ProductCategory EditProductCategory(int id)
        {
            var result = _dbContext.ProductCategories.SingleOrDefault(d => d.Id == id);

            return result ?? null;
        }

        public List<Product> GetAllExpiredProducts()
        {
            var Products = GetAllProducts();
            var expiredProducts = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.ExpiryDate.CompareTo(DateTime.Today) <= 1)
                {
                    expiredProducts.Add(Product);
                }
            });
            return expiredProducts;
        }

        public List<Product> GetAllExpiringProducts(TimeFrame timeFrame)
        {
            var Products = GetAllProducts();
            var expiringProducts = new List<Product>();
            var today = DateTime.Now;
            switch (timeFrame)
            {
                case TimeFrame.MONTHLY:
                    {
                        Products.ForEach(Product =>
                        {
                            if (today.AddMonths(1).Month.Equals(Product.ExpiryDate.Month))
                            {
                                expiringProducts.Add(Product);
                            }
                        });
                        break;
                    }
                case TimeFrame.WEEKLY:
                    {
                        Products.ForEach(Product =>
                        {
                            if (today.AddDays(7).Day.Equals(Product.ExpiryDate.Day))
                            {
                                expiringProducts.Add(Product);
                            }
                        });
                        break;
                    }
                default:
                    {
                        throw new Exception("An Error Occurred");
                    }
            }

            return expiringProducts;
        }

        public List<Product> GetAllProducts()
        {
           var result = _dbContext.Products.Include(d => d.ProductCategory).ToList();
            return result;
        }

        public List<Product> GetAvailableProductFilter(string searchQuery)
        {
            var queries = string.IsNullOrEmpty(searchQuery) ? null : Regex.Replace(searchQuery, @"\s+", " ").Trim().ToLower();
            if (queries == null)
            {
                return GetAvailableProducts();
            }
            return GetAvailableProducts().Where(item => queries.Any(query => (item.ProductName.ToLower().Contains(query)))).ToList();
        }

        public List<Product> GetAvailableProducts()
        {
            var Products = GetAllProducts();
            var availableProducts = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.Quantity > 0)
                {
                    if (Product.ExpiryDate.CompareTo(DateTime.Now) == 1)
                    {
                        availableProducts.Add(Product);
                    }
                }
            }
            );
            return availableProducts;
        }

        public Product GetAvailableProductsById(int id)
        {
            var Product = GetAvailableProducts().Find(d => d.Id == id);

            return Product ?? null;
        }

        public List<Product> GetOutOfStockProducts()
        {
            var Products = GetAllProducts();
            var OutOfStockProducts = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.Quantity < 1)
                {
                    OutOfStockProducts.Add(Product);
                }
            });
            return OutOfStockProducts;
        }

        public Product GetProductById(int id)
        {
            var result = _dbContext.Products.FirstOrDefault(Product => Product.Id == id);

            if (result == null)
                return null;

            return result;
        }

        public List<Product> GetProductsRunningOutOfStock()
        {
            var Products = GetAllProducts();
            var ProductsRunningOutOfStock = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.Quantity < 20)
                {
                    ProductsRunningOutOfStock.Add(Product);
                }
            });
            return ProductsRunningOutOfStock;
        }

        public Task NotifyProductExpirationAsync()
        {
            throw new NotImplementedException();
        }

        public bool RemoveProduct(int id)
        {

            var Product = GetProductById(id);
            if (Product == null)
                return false;
            else
            {
                _dbContext.Products.Remove(_dbContext.Products.Single(d => d.Id == id));
                _dbContext.SaveChanges();
                return true;
            }
        }

        public bool RemoveProductCategory(int id)
        {
            var removeCategory = _dbContext.ProductCategories.SingleOrDefault(category => category.Id == id);

            if (removeCategory == null)
                return false;

            _dbContext.ProductCategories.Remove(_dbContext.ProductCategories.Single(c => c.Id == id));
            _dbContext.SaveChanges();
            return true;
        }

        public void UpdateProduct(Product Product)
        {
            var update = _dbContext.Products.Add(Product);
            _dbContext.Entry(update).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public void UpdateProductCategory(ProductCategory category)
        {
            var update = _dbContext.ProductCategories.Add(category);
            _dbContext.Entry(update).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }
    }
}
