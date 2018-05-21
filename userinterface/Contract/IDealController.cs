using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using userinterface.Models;
using userinterface.Common;
using userinterface.Business;
using Microsoft.AspNetCore.Hosting;

namespace userinterface.Contract
{
    public interface IDealController
    {
        Task<IActionResult> CheckBalance(string invoiceref);
        Task<IActionResult> PayService(RequestBalance data);
        Task<IActionResult> CompensacionPago();
    }
}