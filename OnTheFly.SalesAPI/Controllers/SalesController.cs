using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services;
using System.Text;

namespace OnTheFly.SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "salesReservation";
        private const string QUEUE_SOLD = "sold";

        public SalesController(ConnectionFactory factory)
        {
            _saleService = new();
            _factory = factory;
        }


        [HttpGet] // GET: api/Sales
        public async Task<ActionResult<IEnumerable<Sale>>> GetSale()
        {
            var sales = await _saleService.GetSale();
            return sales == null ? NotFound("Can't find sales") : Ok(sales);
        }


        [HttpGet("{id}")] // GET: api/Sales/5
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _saleService.GetSale(id);

            return sale == null ? NotFound("Can't find sale with id " + id) : Ok(sale);
        }

        [HttpPatch("Sold/{id}")]
        public async Task<ActionResult> SoldSale(int id)
        {
            try
            {
                using (var conn = _factory.CreateConnection())
                {
                    using (var channel = conn.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: QUEUE_SOLD,
                            durable: false,
                            exclusive: false,
                            autoDelete: false);

                        var stringId = JsonConvert.SerializeObject(id);
                        var bytesId = Encoding.UTF8.GetBytes(stringId);

                        channel.BasicPublish(
                            exchange: "",
                            routingKey: QUEUE_SOLD,
                            basicProperties: null,
                            body: bytesId
                            );
                    };
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(SaleDTO dto)
        {
            try
            {
                using var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();

                // Declare the queue
                channel.QueueDeclare(
                    queue: QUEUE_NAME,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                string saleAsStr = JsonConvert.SerializeObject(dto);
                var saleAsBytes = Encoding.UTF8.GetBytes(saleAsStr);


                channel.BasicPublish(
                    exchange: "",
                    routingKey: QUEUE_NAME,
                    basicProperties: null,
                    body: saleAsBytes
                    );

                return Accepted();
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public void RemoveSale(int id)
        {
            _saleService.RemoveSale(id);
        }

    }
}
