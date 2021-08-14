using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IProductService
    {

        Task<PagedList<Product>> GetAllProducts();
        Task<PagedList<Product>> GetAllExpiringProducts(TimeFrame timeFrame);
        Task<PagedList<Product>> GetAllExpiredProducts();
        Task<PagedList<Product>> GetProductsRunningOutOfStock();
        Task<PagedList<Product>> GetOutOfStockProducts();
        Product GetProductById(int id);
        ProductCategory GetProductCategoryById(int id);
        List<ProductCategory> AllCategories();
        void AddProduct(Product Product);
        bool RemoveProduct(int id);
        Product EditProduct(int id);
        int DateComparison(DateTime FirstDate, DateTime SecondDate);

        void AddProductCategory(ProductCategory category);
        bool RemoveProductCategory(int id);

        Task<PagedList<Product>> GetAvailableProducts();
        Product GetAvailableProductsById(int id);

        Task<PagedList<Product>> GetAvailableProductFilter(string searchQuery);

        void UpdateProduct(Product Product);

        ProductCategory EditProductCategory(int id);
        void UpdateProductCategory(ProductCategory category);

    }
}
