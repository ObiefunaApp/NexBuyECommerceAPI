using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;
using AutoMapper;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IShoppingCartService _ShoppingCartService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IShoppingCartService ShoppingCartService, IMapper mapper)
        {
            _ShoppingCartService = ShoppingCartService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("Invoice", Name = "GetInvoice")]
        public ActionResult Invoice(bool? payWithCash)
        {
            if (payWithCash != null)
            {
                payWithCash = true;
            }
            return Ok();
        }


        //[HttpPost()]
        //public ActionResult Checkout(OrderViewModel viewModel)
        //{
        //    var userId = User.Identity.GetUserId();
        //    var items = _ShoppingCartService.GetProductCartItems(userId, CartStatus.ACTIVE);

        //    if (!items.Any())
        //    {
        //        ModelState.AddModelError("", @"Your cart is empty");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var clearedBy = User.Identity.GetUserName();

        //        var order = _orderService.CreateOrder(_mapper.Map<OrderViewModel, Order>(viewModel), userId, clearedBy);
        //        _ShoppingCartService.RefreshCart(userId);
        //        return RedirectToAction("ProcessPayment", "Payment", new { orderId = order.OrderId });

        //    }
        //    return CreatedAtRoute("Invoice", viewModel);

        //}

        [HttpPost]
        public ActionResult CheckoutWithCash(OrderViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            var items = _ShoppingCartService.GetProductCartItems(userId, CartStatus.ACTIVE);

            if (!items.Any())
            {
                ModelState.AddModelError("", @"Your cart is empty");
            }

            if (ModelState.IsValid)
            {
                var clearedBy = User.Identity.GetUserName();

                _orderService.CreateOrder(_mapper.Map<OrderViewModel, Order>(viewModel), userId, clearedBy);
                _ShoppingCartService.RefreshCart(userId);

                RedirectToAction("CheckoutComplete");
            }
            return BadRequest(new { message = "Invalid Purchase" });

        }

        [HttpGet("Complete", Name = "CheckoutComplete")]
        public ActionResult CheckoutComplete()
        {
            //ViewBag.CheckoutCompleteMessage = "Product Dispensed";
            return Ok(new { message = "Checkout complete!" });
        }
    }
}
