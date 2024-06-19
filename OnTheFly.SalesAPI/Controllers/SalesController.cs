﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.SalesAPI.Data;
using Repositories;
using Services;

namespace OnTheFly.SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly OnTheFlySalesAPIContext _context;
        private readonly SaleService _saleService;

        public SalesController(OnTheFlySalesAPIContext context)
        {
            _context = context;
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

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
          if (_context.Sale == null)
          {
              return Problem("Entity set 'OnTheFlySalesAPIContext.Sale'  is null.");
          }
            //_context.Sale.Add(sale);
            //  await _context.SaveChangesAsync();

            SaleService sS = new SaleService();
            sS.Post(sale);

            return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public void RemoveSale(int id)
        {
            new SaleRepository().RemoveSale(id);           
        }

        //private CanceledSale CopyCanceledSale(Sale sale)
        //{
        //    var canceledSale = new CanceledSale
        //    {
        //        Id = sale.Id,
        //        Flight = sale.Flight,
        //        Passengers = sale.Passengers,
        //        Reserved = sale.Reserved,
        //        Sold = sale.Sold
        //    };
        //    return canceledSale;
        //}
        private bool SaleExists(int id)
        {
            return (_context.Sale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
