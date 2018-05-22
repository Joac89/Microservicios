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
            var response = new ResponseDeal();

            try
            {
                var service = await client.GetAsync($"http://localhost:9002/api/v1/routing/getroutingdeal/{invoiceRef}");

                if (service.IsSuccessStatusCode)
                {
                    var json = await service.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ResponseDeal>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Routing Service: " + ex.Message);
            }

            return response;
        }

        public async Task<TransformResult> ConsumeRest(string url)
        {
            var client = new HttpClient();
            var result = new TransformResult();
            try
            {
                var service = await client.GetAsync(url);

                if (service.IsSuccessStatusCode)
                {
                    result.Result = await service.Content.ReadAsStringAsync();
                    result.MediaType = service.Content.Headers.ContentType.MediaType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Rest: " + ex.Message);
            }
            return result;
        }

        public async Task<TransformResult> ConsumeRest(string url, string json, HttpUseMethod method)
        {
            var client = new HttpClient();
            var result = new TransformResult();

            var request = new HttpRequestMessage
            {
                Method = method == HttpUseMethod.POST ? HttpMethod.Post : HttpMethod.Delete,
                RequestUri = new Uri(url),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            try
            {
                var service = await client.SendAsync(request);

                if (service.IsSuccessStatusCode)
                {
                    result.Result = await service.Content.ReadAsStringAsync();
                    result.MediaType = service.Content.Headers.ContentType.MediaType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Rest: " + ex.Message);
            }

            return result;
        }

        public async Task<TransformResult> ConsumeSoap(string url, string request)
        {
            var result = new TransformResult();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Consume Soap: " + ex.Message);
            }

            return result;
        }
    }
}