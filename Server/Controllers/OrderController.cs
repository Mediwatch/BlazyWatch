using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Mediwatch.Shared.Models;
using Server;

namespace  Mediwatch.Server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class OrderController: ControllerBase
    {
        private readonly DbContextMediwatch _context;
        public OrderController(DbContextMediwatch context)
        {
            _context = context;
        }

        //GET: /Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<orderInfo>>> GetOrder(){
                return await _context.orderInfos.ToListAsync();
        }

        //GET /Order/{id}
        [HttpGet ("{id}")]

        public async Task<ActionResult<orderInfo>> GetOrder(int id)
        {
            var orderResult = await _context.orderInfos.FindAsync(id);
            if (orderResult == null)
            {
                return NotFound();
            }

            return orderResult;
        }
    
        //POST /Order/
        [HttpPost]
        public async Task<ActionResult<orderInfo>> PostOrder(orderInfo orderBody){
            // orderInfo orderBody = new orderInfo();
            orderBody.createAt = DateTime.Now;
            _context.orderInfos.Add(orderBody);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = orderBody.id }, orderBody);
        }
    }
}