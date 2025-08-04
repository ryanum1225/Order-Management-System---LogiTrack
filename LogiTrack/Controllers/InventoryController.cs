using LogiTrack.Data;
using LogiTrack.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly LogiTrackContext _context;
        public InventoryController(LogiTrackContext Context) => _context = Context;


        // GET: "/api/inventory"
        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.InventoryItems.ToListAsync();
            return Ok(items);
        }


        // GET: "/api/inventory/{id}"
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _context.InventoryItems.Where(ii => ii.ItemId == id).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);

        }


        // POST: "/api/inventory"
        [Authorize(Policy = "EditorPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateItem(InventoryItem itemInput)
        {
            _context.Add(itemInput);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = itemInput.ItemId }, itemInput);
        }


        // DELETE: "/api/inventory/{id}"
        [Authorize(Policy = "EditorPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.InventoryItems.Where(ii => ii.ItemId == id).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}