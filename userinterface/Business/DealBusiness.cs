using userinterface.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Text;
using System;
using Newtonsoft.Json;

namespace userinterface.Business
{
    public class DealBusiness
    {
        //private string kafkaEndpointSend = "172.17.0.1:9092"; 
        //private string kafkaEndpointGet = "172.17.0.1:9093";
        private string kafkaEndpointSend = "localhost:9092";
        private string kafkaEndpointGet = "localhost:9093";
        private string kafkaTopicSend = "broker-replicated";
        private string kafkaTopicGet = "broker-replicated2";
        Dictionary<string, object> producerConfigSend;
        Dictionary<string, object> producerConfigGet;

        public DealBusiness()
        {
            producerConfigSend = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpointSend } };
            producerConfigGet = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpointGet } };
        }

        public async Task<ResponseBalance> CheckBalance(string invoiceRef)
        {
            kafkaProducer(invoiceRef);

            var message = KafkaConsumer();
            var response = new ResponseBalance();

            try
            {
                var serialize = JsonConvert.DeserializeObject<Balance>(message);

                response.Code = 200;
                response.Data = serialize;
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Data = null;
                response.Message = ex.Message;
            }
            
            return await Task.Run(() => response);
        }

        public async Task<ResponsePayment> PayService(RequestBalance data)
        {
            var send = $"{data.InvoiceRef}#{data.BalanceToPay}";
            kafkaProducer(send);

            var message = KafkaConsumer();
            var response = new ResponsePayment();

            try
            {
                var serialize = JsonConvert.DeserializeObject<Payment>(message);

                response.Code = 200;
                response.Data = serialize;
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Data = null;
                response.Message = ex.Message;
            }


            return await Task.Run(() => response);
        }

        public async Task<ResponsePayment> Compensate(RequestBalance data)
        {
            var send = $"{data.InvoiceRef}#{data.BalanceToPay}";
            kafkaProducer(send);

            var message = KafkaConsumer();
            var response = new ResponsePayment();

            try
            {
                var serialize = JsonConvert.DeserializeObject<Payment>(message);

                response.Code = 200;
                response.Data = serialize;
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Data = null;
                response.Message = ex.Message;
            }


            return await Task.Run(() => response);
        }


        private void kafkaProducer(string message)
        {
            using (var producer = new Producer<Null, string>(producerConfigSend, null, new StringSerializer(Encoding.UTF8)))
            {
                var dr = producer.ProduceAsync(kafkaTopicSend, null, message).Result;
                
            }
        }
        private string KafkaConsumer()
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
                  => message = msg.Value; //Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");

                consumer.OnError += (_, error)
                  => message = error.ToString();

                consumer.OnConsumeError += (_, msg)
                  => message = msg.Error.ToString();

                consumer.Subscribe(kafkaTopicGet);

                while (true)
                {
                    //var x = "";
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        //x = message;
                        return message;
                    }
                    /*else
                    {
                        return x;
                    }*/
                    message = "";
                }
            }
        }
    }
}