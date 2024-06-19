﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.SalesAPI.Data;
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

        [HttpPatch("/Sold/{id}")]
        public async Task<ActionResult> SoldSale(int id)
        {
            bool isSold = await _saleService.SoldSale(id);
            if (isSold)
                return Ok();
            return Problem("Unable to complete sale");

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

            try
            {
                var result = await _saleService.Put(sale);
                if (result)
                    return Ok();

                return Problem("Unable to update sale");
            }
            catch (Exception)
            {

                throw;
            }



        }
    }
}
