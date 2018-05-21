using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using userinterface.Models;
using userinterface.Common;
using userinterface.Business;
using Microsoft.AspNetCore.Hosting;
using userinterface.Contract;
using System.ComponentModel.DataAnnotations;

namespace userinterface.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class DealController : Controller //, IDealController
    {
        private string respBalanceSchema = "";
        private string respPaymentSchema = "";
        private string reqBalanceSchema = "";

        public DealController(IHostingEnvironment hostingEnvironment)
        {
            var webRootPath = hostingEnvironment.WebRootPath;

            reqBalanceSchema = webRootPath + "/Schema/request-balance-schema.json";
            respBalanceSchema = webRootPath + "/Schema/response-balance-schema.json";
            respPaymentSchema = webRootPath + "/Schema/response-payment-schema.json";
        }

        /// <summary>Consultar estado de factura</summary>
        /// <remarks>Consulta el valor a pagar de la factura solicitada</remarks>
        /// <param name="invoiceref"></param>
        /// <returns>Retorna los datos de la factura solicitada</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>    
        [HttpGet]
        [Route("checkbalance/{invoiceref}")]
        [ProducesResponseType(typeof(ResponseBalance), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckBalance(string invoiceref)
        {
            var serv = new DealBusiness();
            var result = await serv.CheckBalance(invoiceref);

            if (result.Code == 200)
            {
                var validate = SchemaEngine.Validate<ResponseBalance>(result, respBalanceSchema);

                if (!validate)
                {
                    result.Code = 500;
                    result.Message = "Invalid result data schema";
                    result.Data = null;

                    return StatusCode(500, result);
                }
            }
            else
            {
                return StatusCode(result.Code, result);
            }

            return Ok(result);
        }

        /// <summary>Pagar factura</summary>
        /// <remarks>Paga el valor de la factura solicitada</remarks>
        /// <param name="data"></param>
        /// <returns>Retorna el resultado de la factura pagada o no pagada</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("payservice")]
        [ProducesResponseType(typeof(ResponsePayment), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PayService([FromBody] RequestBalance data)
        {
            var result = new ResponsePayment();
            var validateRequest = SchemaEngine.Validate<RequestBalance>(data, reqBalanceSchema);

            if (validateRequest)
            {
                var serv = new DealBusiness();
                result = await serv.PayService(data);

                if (result.Code == 200)
                {
                    var validate = SchemaEngine.Validate<ResponsePayment>(result, respPaymentSchema);

                    if (!validate)
                    {
                        result.Code = 500;
                        result.Message = "Invalid result data schema";
                        result.Data = null;

                        return StatusCode(500, result);
                    }
                }
                else
                {
                    return StatusCode(result.Code, result);
                }

                return Ok(result);
            }
            else
            {
                result.Code = 500;
                result.Message = "Invalid params schema";
                result.Data = null;

                return StatusCode(500, result);
            }
        }

        /// <summary>Compensar factura</summary>
        /// <remarks>Compensa el valor de la factura solicitada</remarks>
        /// <param name="data"></param>
        /// <returns>Retorna el resultado de la factura compensada</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("compensate")]
        [ProducesResponseType(typeof(ResponsePayment), 200)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> Compensate([FromBody] RequestBalance data)
        {
            var result = new ResponsePayment();
            var validateRequest = SchemaEngine.Validate<RequestBalance>(data, reqBalanceSchema);

            if (validateRequest)
            {
                var serv = new DealBusiness();
                result = await serv.Compensate(data);

                if (result.Code == 200)
                {
                    var validate = SchemaEngine.Validate<ResponsePayment>(result, respPaymentSchema);

                    if (!validate)
                    {
                        result.Code = 500;
                        result.Message = "Invalid result data schema";
                        result.Data = null;

                        return StatusCode(500, result);
                    }
                }
                else
                {
                    return StatusCode(result.Code, result);
                }

                return Ok(result);
            }
            else
            {
                result.Code = 500;
                result.Message = "Invalid params schema";
                result.Data = null;

                return StatusCode(500, result);
            }
        }
    }
}
