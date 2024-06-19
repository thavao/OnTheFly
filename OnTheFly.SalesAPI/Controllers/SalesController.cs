using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace OnTheFly.SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SaleService _saleService;

        public SalesController()
        {
            _saleService = new();
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
                bool isSold = await _saleService.SoldSale(id);

                if (isSold)
                    return NoContent();

                return Problem("Unable to complete sale");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            _saleService.Post(sale);

            return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public void RemoveSale(int id)
        {
            _saleService.RemoveSale(id);
        }

    }
}
