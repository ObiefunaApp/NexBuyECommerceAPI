using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NexBuyECommerceAPI.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly ITransactionService _paymentService;
        
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<ActionResult> ProcessPayment(int orderId)
        {
            try
            {
                var result = await _paymentService.InitiatePayment(orderId);
                return Redirect(result.checkoutUrl);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> VerifyPayment(string paymentReference)
        {
            try
            {
                var response = await _paymentService.VerifyPayment(paymentReference);
                if (response)
                {
                    ViewBag.PaymentResponse = true;
                    return RedirectToAction("Index", "Home", new { paymentCompleted = "True" });
                }
                ViewBag.PaymentResponse = false;
                return RedirectToAction("Index", "Home", new { paymentCompleted = "False" });

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
