using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using dispatcher.Models;
using Newtonsoft.Json;

namespace dispatcher.Common
{
    public class ServiceRouting
    {
        public enum HttpUseMethod
        {
            POST = 1,
            DELETE = 2
        }
        public async Task<ResponseDeal> GetRouting(string invoiceRef)
        {
            var client = new HttpClient();
            var service = await client.GetAsync($"http://localhost:9002/api/v1/routing/getroutingdeal/{invoiceRef}");
            var response = new ResponseDeal();

            if (service.IsSuccessStatusCode)
            {
                var json = await service.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ResponseDeal>(json);
            }

            return response;
        }

        public async Task<TransformResult> ConsumeRest(string url)
        {
            var client = new HttpClient();
            var result = new TransformResult();

            var service = await client.GetAsync(url);


            if (service.IsSuccessStatusCode)
            {
                result.Result = await service.Content.ReadAsStringAsync();
                result.MediaType = service.Content.Headers.ContentType.MediaType;
            }

            return result;
        }

        public async Task<TransformResult> ConsumeRest(string url, string json, HttpUseMethod method)
        {
            var client = new HttpClient();
            var result = new TransformResult();
            //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = method == HttpUseMethod.POST ? HttpMethod.Post : HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            //var service = await client.PostAsync(url, stringContent);
            var service = await client.SendAsync(request);

            if (service.IsSuccessStatusCode)
            {
                result.Result = await service.Content.ReadAsStringAsync();
                result.MediaType = service.Content.Headers.ContentType.MediaType;
            }

            return result;
        }

        public async Task<TransformResult> ConsumeSoap(string url, string request)
        {
            var result = new TransformResult();

            using (var client = new HttpClient())
            {
                var action = url.Split("#")[1];
                var newUrl = url.Split("#")[0];

                client.DefaultRequestHeaders.Add("SOAPAction", action);

                var content = new StringContent(request, Encoding.UTF8, "text/xml");
                using (var response = await client.PostAsync(newUrl, content))
                {
                    var soapResponse = await response.Content.ReadAsStringAsync();

                    result.Result = soapResponse;
                    result.MediaType = "text/xml";
                }
            }

            return result;
        }
    }
}