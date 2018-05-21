using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using routingdeal.Models;

namespace routingdeal.Business
{
    public class RoutingBusiness
    {
        private HttpContext _httpContext;
        public RoutingBusiness(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public async Task<ResponseDeal> AddRoutingDeal(RequestDeal data)
        {
            var context = _httpContext.RequestServices.GetService(typeof(RoutingDao)) as RoutingDao;
            var response = context.AddRoutingDeal(data);

            return await Task.Run(() => response);
        }

        public async Task<ResponseDeal> GetRoutingDeal(string invoiceref)
        {
            var context = _httpContext.RequestServices.GetService(typeof(RoutingDao)) as RoutingDao;
            var response = context.GetRoutingDeal(invoiceref);

            return await Task.Run(() => response);
        }

        public async Task<ResponseUpdate> UpdateRoutingDeal(RequestUpdate data)
        {
            var context = _httpContext.RequestServices.GetService(typeof(RoutingDao)) as RoutingDao;
            var response = context.UpdateRoutingDeal(data);

            return await Task.Run(() => response);
        }
    }
}