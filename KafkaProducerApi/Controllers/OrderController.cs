using Confluent.Kafka;
using Kafka.Library;
using KafkaProducerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using static Confluent.Kafka.ConfigPropertyNames;

namespace KafkaProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        KafkaProducer _producer; 
        public IConfiguration configRoot { get; }
        public OrderController(KafkaProducer producer, IConfiguration configuration)
        {
            _producer = producer;
            configRoot = configuration;
        }
     
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] Order order)
        {
            try
            {
                var BootstrapServers = configRoot.GetValue<string>("BootstrapServers");
                var ClientId = configRoot.GetValue<string>("ClientId");

                _producer.InitializeProduce(BootstrapServers, ClientId);

                var message = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value =JsonConvert.SerializeObject(order)
                };

                _producer.Produce("order-topic", message);

                //  _producer.Produce("order-topic", message);


                return Ok("Order placed successfully and sent to Kafka.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to place the order: {ex.Message}");
            }
        }
    }
}

