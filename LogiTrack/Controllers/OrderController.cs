using LogiTrack.Data;
using LogiTrack.Entities;
using LogiTrack.Dto;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly LogiTrackContext _context;
        private readonly IMemoryCache _cache;
        public OrderController(LogiTrackContext Context, IMemoryCache Cache)
        {
            _context = Context;
            _cache = Cache;

            // Load all inventory items into cache.
            _cache.Set("ItemList", _context.Orders.ToList(),
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    Size = 5
                });
        }


        // GET: "api/orders"
        [Authorize(Policy = "EditorPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            // Try accessing the cache.
            if (!_cache.TryGetValue("ItemList", out List<Orders>? items))
            {
                // Retrieve items from database, and set the cache.
                items = await _context.Orders.ToListAsync();

                _cache.Set("ItemList", _context.Orders.ToList(),
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    Size = 5
                });
        
            return Ok(orders);
        }


        // GET: "api/orders/{id}"
        [Authorize(Policy = "EditorPolicy")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.Where(ord => ord.OrderId == id).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            await _context.Entry(order)
                .Collection(order => order.Items)
                .LoadAsync();

            return Ok(order);

        }


        // POST: "api/orders/"
        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(AddOrderDTO newOrder)
        {
            var order = new Order
            {
                CustomerName = newOrder.CustomerName,
                DatePlaced = DateTime.Now
            };

            if (newOrder.Items != null)
            {
                foreach (var item in newOrder.Items)
                {
                    order.AddItem(item);
                }
            }

            _context.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }


        // DELETE: "api/orders/{id}"
        [Authorize(Policy = "EditorPolicy")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.Where(ord => ord.OrderId == id).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
