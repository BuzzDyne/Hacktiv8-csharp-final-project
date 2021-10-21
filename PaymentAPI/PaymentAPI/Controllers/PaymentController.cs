using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PaymentAPI.Data;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PaymentController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Payment
        [HttpGet]
        public ActionResult<IEnumerable<PaymentItem>> GetPaymentItems()
        {
            return _context.Payment.ToList();
        }

        // GET: api/Payment/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<PaymentItem>> GetPaymentItem(int id)
        {
            var paymentItem = await _context.Payment.FindAsync(id);

            if (paymentItem == null)
            {
                return NotFound();
            }

            return paymentItem;
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<ActionResult<PaymentItem>> PostPaymentItem(PaymentItem paymentItem)
        {
            _context.Payment.Add(paymentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentItems", new { id = paymentItem.Id }, paymentItem);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentItem(int id, PaymentItem paymentItem)
        {
            if (id != paymentItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentItemExists(id))
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

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentItem(int id)
        {
            var paymentItem = await _context.Payment.FindAsync(id);
            if (paymentItem == null)
            {
                return NotFound();
            }

            _context.Payment.Remove(paymentItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentItemExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }


    }
}
