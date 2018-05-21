using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using dispatcher.Common;
using dispatcher.Models;
using Newtonsoft.Json;

namespace dispatcher
{
    public class DispatcherModule
    {
        private static string kafkaEndpointGet = "localhost:9092";
        private static string kafkaEndpointSend = "localhost:9093";
        private static string kafkaTopicGet = "broker-replicated";
        private static string kafkaTopicSend = "broker-replicated2";
        private static Dictionary<string, object> producerConfigSend;
        private static Dictionary<string, object> producerConfigGet;

        static void Main(string[] args)
        {
            Console.WriteLine("Dispatcher Started");

            producerConfigSend = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpointSend } };
            producerConfigGet = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpointGet } };

            KafkaConsumer();
        }

        private static void kafkaProducer(string message)
        {
            var serv = new ServiceRouting();
            var routing = serv.GetRouting(message).Result;
            var json = "";
            var data = new TransformResult();
            var trans = new Transformation();
            var req = "";
            var url = "";
            //create connetion
            if (routing.Code == 200)
            {
                switch (routing.Data.Type)
                {
                    case "REST-GET":
                        url = $"{routing.Data.Url}/{message}";
                        data = serv.ConsumeRest(url).Result;

                        json = trans.Execute(data, routing.Data.Template);
                        break;
                    case "REST-POST":
                        var vrest = message.Split('#');
                        url = $"{routing.Data.Url}/{vrest[0]}";

                        req = trans.CreateRequest(routing.Data.RequestTemplate, routing.Data.NumRequest, vrest);
                        data = serv.ConsumeRest(url, req, ServiceRouting.HttpUseMethod.POST).Result;

                        json = trans.Execute(data, routing.Data.Template);
                        break;
                    case "REST-DELETE":
                        var vdel = message.Split('#');
                        url = $"{routing.Data.Url}/{vdel[0]}";

                        req = trans.CreateRequest(routing.Data.RequestTemplate, routing.Data.NumRequest, vdel);
                        data = serv.ConsumeRest(url, req, ServiceRouting.HttpUseMethod.DELETE).Result;

                        json = trans.Execute(data, routing.Data.Template);
                        break;
                    case "SOAP-GET":
                        req = trans.CreateRequest(routing.Data.RequestTemplate, routing.Data.NumRequest, message);
                        data = serv.ConsumeSoap(routing.Data.Url, req).Result;

                        json = CallSOAP(data.Result, routing.Data.Template);
                        break;
                    case "SOAP-POST":
                        var vpost = message.Split('#');
                        req = trans.CreateRequest(routing.Data.RequestTemplate, routing.Data.NumRequest, vpost);
                        data = serv.ConsumeSoap(routing.Data.Url, req).Result;

                        json = CallSOAP(data.Result, routing.Data.Template);
                        break;
                }
            }

            using (var producer = new Producer<Null, string>(producerConfigSend, null, new StringSerializer(Encoding.UTF8)))
            {
                var dr = producer.ProduceAsync(kafkaTopicSend, null, json).Result;
                Console.WriteLine($"Message send to kafka: {json}");
            }
        }
        private static string KafkaConsumer()
        {
            var message = "";
            var conf = new Dictionary<string, object>
            {
                { "group.id", "test-consumer-group" },
                { "bootstrap.servers", kafkaEndpointGet },
                { "auto.commit.interval.ms", 5000 },
                { "auto.offset.reset", "earliest" }
            };

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg)
                  => message = msg.Value;

                consumer.OnError += (_, error)
                  => message = error.ToString();

                consumer.OnConsumeError += (_, msg)
                  => message = msg.Error.ToString();

                consumer.Subscribe(kafkaTopicGet);

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        Console.WriteLine($"Message receive from Kafka: {message}");

                        kafkaProducer(message);
                        message = "";
                    }
                }
            }
        }

        private static string CallSOAP(string result, string template)
        {
            var data = new TransformResult();
            var trans = new Transformation();
            var simpleXml = trans.RemoveAllNamespacesXml(XElement.Parse(result)).ToString();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(simpleXml);

            data.MediaType = "application/json";
            data.Result = JsonConvert.SerializeXmlNode(xmlDoc);

            return trans.Execute(data, template);
        }
    }
}
