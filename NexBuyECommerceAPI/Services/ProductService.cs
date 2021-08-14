using Microsoft.EntityFrameworkCore;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;
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

        public async Task<PagedList<Product>> GetAllExpiredProducts()
        {
            var Products = await GetAllProducts();
            var expiredProducts = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.ExpiryDate.CompareTo(DateTime.Today) <= 1)
                {
                    expiredProducts.Add(Product);
                }
            });
            return (PagedList<Product>)expiredProducts;
        }

        public async Task<PagedList<Product>> GetAllExpiringProducts(TimeFrame timeFrame)
        {
            var Products = await GetAllProducts();
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

            return (PagedList<Product>)expiringProducts;
        }

        public async Task<PagedList<Product>> GetAllProducts()
        {
           var result = await _dbContext.Products.Include(d => d.ProductCategory).ToListAsync();
            return (PagedList<Product>)result;
        }

        public async Task<PagedList<Product>> GetAvailableProductFilter(string searchQuery)
        {
            var queries = string.IsNullOrEmpty(searchQuery) ? null : Regex.Replace(searchQuery, @"\s+", " ").Trim().ToLower();
            var res = await GetAvailableProducts();
            if (queries == null)
            {
                return res;
            }
            var result = res.Where(item => queries.Any(query => (item.ProductName.ToLower().Contains(query)))).ToList();

            return (PagedList<Product>)result;
        }

        public async Task<PagedList<Product>> GetAvailableProducts()
        {
            var Products = await GetAllProducts();
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

            return (PagedList<Product>)availableProducts;
        }

        public Product GetAvailableProductsById(int id)
        {
            var Product = GetAvailableProducts().Result.Find(d => d.Id == id);

            return Product ?? null;
        }

        public async Task<PagedList<Product>> GetOutOfStockProducts()
        {
            var Products = await GetAllProducts();
            var OutOfStockProducts = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.Quantity < 1)
                {
                    OutOfStockProducts.Add(Product);
                }
            });
            return (PagedList<Product>)OutOfStockProducts;
        }

        public Product GetProductById(int id)
        {
            var result = _dbContext.Products.FirstOrDefault(Product => Product.Id == id);

            if (result == null)
                return null;

            return result;
        }

        public ProductCategory GetProductCategoryById(int id)
        {
            var result = _dbContext.ProductCategories.FirstOrDefault(Product => Product.Id == id);

            if (result == null)
                return null;

            return result;
        }

        public async Task<PagedList<Product>> GetProductsRunningOutOfStock()
        {
            var Products = await GetAllProducts();
            var ProductsRunningOutOfStock = new List<Product>();
            Products.ForEach(Product =>
            {
                if (Product.Quantity < 20)
                {
                    ProductsRunningOutOfStock.Add(Product);
                }
            });
            return (PagedList<Product>)ProductsRunningOutOfStock;
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
