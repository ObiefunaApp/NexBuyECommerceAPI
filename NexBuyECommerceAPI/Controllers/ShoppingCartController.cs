using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _ShoppingCartService;
       // private readonly IProductService _ProductService;

        public ShoppingCartController(IShoppingCartService ShoppingCartService/*, IProductService ProductService*/)
        {
            _ShoppingCartService = ShoppingCartService;
            //_ProductService = ProductService;
        }

        
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var ProductCartCountTotal = _ShoppingCartService.GetProductCartTotalCount(userId);
            var ProductCartViewModel = new ShoppingCartViewModel
            {
                CartItems = _ShoppingCartService.GetProductCartItems(userId, CartStatus.ACTIVE),
                ProductCartItemsTotal = ProductCartCountTotal,
                ProductCartTotal = _ShoppingCartService.GetProductCartSumTotal(userId),

            };
            return Ok(ProductCartViewModel);
        }
        public ActionResult GetProduct(int id)
        {
            var Product = _ShoppingCartService.GetProductById(id);
            return Ok(Product);
        }

        public ActionResult AddToShoppingCart(int id)
        {
            var userId = User.Identity.GetUserId();
            var selectedProduct = _ShoppingCartService.GetProductById(id);

            //    var prescribeVM = Mapper.Map<ProductPrescriptionViewModel>(selectedProduct);
            try
            {
                if (selectedProduct == null)
                {
                    return NotFound();
                }

                _ShoppingCartService.AddToCart(selectedProduct, userId);
                return Ok(selectedProduct);
            }
            catch (Exception ex)
            {
               // ViewBag.Error = ex.Message;
                return BadRequest(new { message = ex });
            }

        }


        public ActionResult RemoveFromShoppingCart(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var cartItem = _ShoppingCartService.GetProductCartItemById(id);
                var selectedItem = _ShoppingCartService.GetProductById(cartItem.Product.Id);

                if (selectedItem != null)
                {
                    _ShoppingCartService.RemoveFromCart(selectedItem, userId);
                }

                return NoContent();
            }
            catch (Exception e)
            {
              
                return BadRequest(new { message = e });
            }
        }

        public ActionResult RemoveAllCart()
        {
            var userId = User.Identity.GetUserId();
            _ShoppingCartService.ClearCart(userId);
            return NoContent();
        }
    }
}
