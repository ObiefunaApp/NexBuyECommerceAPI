using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NexBuyECommerceAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {

        }


        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>
            {
                new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"),
                new LinkDto(Url.Link("GetAllProducts", new { }), "authors", "GET"),
                new LinkDto(Url.Link("GetSalesReport", new { }), "books", "GET")
            };
            return Ok(links);

        }
    }
}
