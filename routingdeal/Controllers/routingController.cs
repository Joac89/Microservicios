using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using routingdeal.Models;
using routingdeal.Common;
using routingdeal.Business;

namespace routingdeal.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class RoutingController : Controller
    {
        private string reqDealSchema = "";
        private string respDealSchema = "";

        public RoutingController(IHostingEnvironment hostingEnvironment)
        {
            var webRootPath = hostingEnvironment.WebRootPath;

            reqDealSchema = webRootPath + "/Schema/request-deal-schema.json";
            respDealSchema = webRootPath + "/Schema/response-deal-schema.json";
        }


        /// <summary>Consultar el routing de un convenio</summary>
        /// <remarks>Consulta el routing para el convenio de la factura solicitada</remarks>
        /// <param name="invoiceref"></param>
        /// <returns>Retorna los datos del convenio a enrutar</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>    
        [HttpGet]
        [Route("getroutingdeal/{invoiceref}")]
        [ProducesResponseType(typeof(ResponseDeal), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRoutingDeal(string invoiceref)
        {
            var key = invoiceref.Length > 3 ? invoiceref.Substring(0, 4): invoiceref;
            var serv = new RoutingBusiness(HttpContext);
            var result = await serv.GetRoutingDeal(key);

            if (result.Code == 200)
            {
                var validate = SchemaEngine.Validate<ResponseDeal>(result, respDealSchema);

                if (!validate)
                {
                    result.Code = 500;
                    result.Message = "Invalid Schema";
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

        /// <summary>Registra el routing de un convenio</summary>
        /// <remarks>Registra el routing para el convenio de la factura solicitada</remarks>
        /// <param name="data"></param>
        /// <returns>Retorna los datos del convenio registrado</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>    
        [HttpPost]
        [Route("addroutingdeal")]
        [ProducesResponseType(typeof(ResponseDeal), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddRoutingDeal([FromBody] RequestDeal data)
        {
            var result = new ResponseDeal();
            var validateRequest = SchemaEngine.Validate<RequestDeal>(data, reqDealSchema);

            if (validateRequest)
            {
                var serv = new RoutingBusiness(HttpContext);
                result = await serv.AddRoutingDeal(data);

                if (result.Code == 200)
                {
                    var validate = SchemaEngine.Validate<ResponseDeal>(result, respDealSchema);

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

        /*
        /// <summary>Actualiza el estado del routing de un convenio</summary>
        /// <remarks>Actualiza el estado del routing para el convenio de la factura solicitada</remarks>
        /// <param name="data"></param>
        /// <returns>Retorna los datos del convenio actualizado</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>    
        [HttpPost]
        [Route("updateroutingdeal")]
        [ProducesResponseType(typeof(ResponseDeal), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRoutingDeal([FromBody] RequestUpdate data)
        {
            var result = new ResponseUpdate();
            var validateRequest = SchemaEngine.Validate<RequestUpdate>(data, reqDealSchema);

            if (validateRequest)
            {
                var serv = new RoutingBusiness(HttpContext);
                result = await serv.UpdateRoutingDeal(data);

                if (result.Code == 200)
                {
                    var validate = SchemaEngine.Validate<ResponseUpdate>(result, respDealSchema);

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
                result.Message = "Invalid params Schema";
                result.Data = null;

                return StatusCode(500, result);
            }
        }*/
    }
}
