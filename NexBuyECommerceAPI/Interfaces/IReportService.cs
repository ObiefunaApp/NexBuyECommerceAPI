using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IReportService
    {

        Report GetReportByFunc(Func<Report, bool> func);
        bool GetReportBoolByFunc(Func<Report, bool> func);
        Report CreateReport(TimeFrame timeFrame);

    }
}
