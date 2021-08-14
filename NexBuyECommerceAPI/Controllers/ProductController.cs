using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
		private readonly IProductService _ProductService;
		private readonly IMapper _mapper;
		private readonly IResourceUtil _resourceUtil;
		
		//private readonly ISupplierService _supplierService;

		public ProductController(IProductService ProductService, IMapper mapper, IResourceUtil resourceUtil)
		{
			_ProductService = ProductService;
			_resourceUtil = resourceUtil;
			_mapper = mapper;
			//_supplierService = supplierService;

		}
		// GET: Product
		[HttpGet("index", Name = "GetAllProducts")]
		public async Task<ActionResult> AllProducts([FromQuery] ResourceParameters resourceParams)
		{
			var result = await _ProductService.GetAllProducts();

			var paginationMetadata = new
			{
				totalCount = result.TotalCount,
				pageSize = result.PageSize,
				currentPage = result.CurrentPage,
				totalPages = result.TotalPages,
			};
			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
			var links = _resourceUtil.CreateLinksFoPaginations("GetAllProducts", resourceParams, result.HasNext, result.HasPrevious);

			return Ok(new { value = result, links });
		}

		[HttpGet("Available", Name = "GetAvailableProducts")]
		public async Task<ActionResult> AvailableProductsAsync([FromQuery] ResourceParameters resourceParams)
		{
			
			var result = await _ProductService.GetAvailableProducts();

			var paginationMetadata = new
			{
				totalCount = result.TotalCount,
				pageSize = result.PageSize,
				currentPage = result.CurrentPage,
				totalPages = result.TotalPages,
			};
			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
			var links = _resourceUtil.CreateLinksFoPaginations("GetAllProducts", resourceParams, result.HasNext, result.HasPrevious);

			return Ok(new { value = result, links });
		}

		[HttpGet("Expired", Name = "GetExpiredProducts")]
		public async Task<ActionResult> GetExpiredProducts([FromQuery] ResourceParameters resourceParams)
		{
			var result = await _ProductService.GetAllExpiredProducts();

			var paginationMetadata = new
			{
				totalCount = result.TotalCount,
				pageSize = result.PageSize,
				currentPage = result.CurrentPage,
				totalPages = result.TotalPages,
			};
			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
			var links = _resourceUtil.CreateLinksFoPaginations("GetAllProducts", resourceParams, result.HasNext, result.HasPrevious);

			return Ok(new { value = result, links });
			
		}

		[HttpGet("OutOfStock", Name = "GetOutOfStockProducts")]
		public ActionResult GetOutOfStockProducts()
		{
			var outOfStockProducts = _ProductService.GetOutOfStockProducts();

			return Ok(outOfStockProducts);
		}

		[HttpGet("Search", Name = "GetFilteredProducts")]
		public async Task<ActionResult> FilteredProductsList(string searchString)
		{
			var Products = await _ProductService.GetAvailableProducts();
			var ProductFilter = await _ProductService.GetAvailableProductFilter(searchString);
			if (string.IsNullOrWhiteSpace(searchString) || string.IsNullOrEmpty(searchString))
			{
				var ProductsVM = new ProductSearchViewModel
				{
					Products = Products,
					SearchString = searchString
				};
				return Ok(ProductsVM);
			}
			var ProductsearchVM = new ProductSearchViewModel
			{
				Products = ProductFilter,
				SearchString = searchString
			};
			return Ok(ProductsearchVM);
		}


		public ActionResult UpdateProduct(int id)
		{

			var ProductInDb = _mapper.Map<Product, ProductViewModel>(_ProductService.EditProduct(id));

			if (ProductInDb == null) return NotFound("No Product found");

			ProductInDb.ProductCategory = _ProductService.AllCategories();

			return Ok(ProductInDb);
		}

		public ActionResult SaveProduct(ProductViewModel product)
		{
			if (!ModelState.IsValid)
			{
				product.ProductCategory = _ProductService.AllCategories();
				return Ok(product);
			}

			try
			{

				//ADD A NEW PRODUCT
				if (product.Id == 0)
				{
					bool result = ValidateProduct(product);

					if (!result) return BadRequest("Invalid product");

					var newProduct = _mapper.Map<ProductViewModel, Product>(product);
					_ProductService.AddProduct(newProduct);

				}
				else
				{
					// UPDATE EXISTING PRODUCT

					var getProductInDb = _ProductService.EditProduct(product.Id);

					if (getProductInDb == null)
                    {
						return NotFound("Product not found!");

					}

					bool result = ValidateProduct(product);

					if (!result)
						return BadRequest("Invalid Product");

					_ProductService.UpdateProduct(_mapper.Map(product, getProductInDb));

				}
			
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				return Ok(new { response = ex.Message });

			}
			return Ok(new { response = "success" });

		}

		private void SetModelStateError(ProductViewModel product, string key, string errorMessage)
		{
			ModelState.AddModelError(key, errorMessage);
		
			product.ProductCategory = _ProductService.AllCategories();
		}


        private bool ValidateProduct(ProductViewModel product)
        {
            var expiryDate = _ProductService.DateComparison(DateTime.Today, product.ExpiryDate);

            if (expiryDate >= 0)
            {
                SetModelStateError(product, "ExpiryDate", "Must be later than today");
                return false;
            }
            else if (product.Quantity <= 0)
            {
                SetModelStateError(product, "Quantity", "Quantity should be greater than zero");
                return false;
            }
            else if (product.Price <= 0)
            {
                SetModelStateError(product, "Price", "Price should be greater than zero");
                return false;
            }
            return true;
        }

      
        [HttpPost]
		public ActionResult SaveProductCategory(ProductCategoryViewModel category)
		{
			if (ModelState.IsValid)
			{

				if (string.IsNullOrWhiteSpace(category.CategoryName))
				{
					ModelState.AddModelError("Category Name", @"Please input category");
					//return Json(new { response = "failure", cat = category }, JsonRequestBehavior.AllowGet);

					return Ok(category);

				}

				var cate = _mapper.Map<ProductCategoryViewModel, ProductCategory>(category);
				_ProductService.AddProductCategory(cate);

				return Ok(cate);

			}
			
			return BadRequest(new { message = "Failed" });
		}

		
		public ActionResult ViewProductCategory(int id)
		{
			var category = _ProductService.GetProductCategoryById(id);

				return Ok(category);
		}

		public ActionResult RemoveProduct(int id)
		{
			try
			{
				var ProductInDb = _ProductService.GetProductById(id);

				// if the Product is not found
				if (ProductInDb == null)
				{
					return NotFound("Product does not exist");
				}
				_ProductService.RemoveProduct(id);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
			return NoContent();
		}

		[HttpGet("ProductCategories", Name = "GetAllProductCategories")]
		public ActionResult ListProductCategories()
		{
			return Ok(_ProductService.AllCategories());
		}

		public ActionResult RemoveProductCategory(int id)
		{
			var removeCategory = _ProductService.RemoveProductCategory(id);
			if (!removeCategory)
				return NotFound("Category does not exist");

			_ProductService.RemoveProductCategory(id);
			return NoContent();
		}


		[HttpPost]
		public ActionResult UpdateProductCategory(EditCategoryViewModel category)
		{

			_ProductService.UpdateProductCategory(_mapper.Map<ProductCategory>(category));
			return Ok(new { response = "success" });

		}

		[HttpGet("ProductDetails", Name = "GetProductById")]
		public ActionResult ViewProduct(int id)
		{
			var ProductInDb = _ProductService.EditProduct(id);

			if (ProductInDb == null) return NotFound("No Product found");

			return Ok(ProductInDb);

		}
	}
}
