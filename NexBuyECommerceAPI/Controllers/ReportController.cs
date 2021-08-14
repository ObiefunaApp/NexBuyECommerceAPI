using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexBuyECommerceAPI.Entities.Enums;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexBuyECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        // GET
        public ActionResult Index(TimeFrame timeFrame)
        {
            var report = _mapper.Map<ReportViewModel>(_reportService.CreateReport(timeFrame));
            return Ok(report);
        }
    }
}
