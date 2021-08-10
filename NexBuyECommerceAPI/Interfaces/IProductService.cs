﻿using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IProductService
    {

        List<Product> GetAllProducts();
        List<Product> GetAllExpiringProducts(TimeFrame timeFrame);
        List<Product> GetAllExpiredProducts();
        List<Product> GetProductsRunningOutOfStock();
        List<Product> GetOutOfStockProducts();
        Product GetProductById(int id);
        List<ProductCategory> AllCategories();
        void AddProduct(Product Product);
        bool RemoveProduct(int id);
        Product EditProduct(int id);
        int DateComparison(DateTime FirstDate, DateTime SecondDate);

        void AddProductCategory(ProductCategory category);
        bool RemoveProductCategory(int id);

        List<Product> GetAvailableProducts();
        Product GetAvailableProductsById(int id);

        List<Product> GetAvailableProductFilter(string searchQuery);

        void UpdateProduct(Product Product);

        ProductCategory EditProductCategory(int id);
        void UpdateProductCategory(ProductCategory category);
        Task NotifyProductExpirationAsync();

    }
}
