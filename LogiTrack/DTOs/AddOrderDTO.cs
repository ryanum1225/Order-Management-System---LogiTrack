using LogiTrack.Entities;

namespace LogiTrack.Dto
{
    public class AddOrderDTO
    {
        public required string CustomerName { get; set; }
        public List<InventoryItem> Items { get; } = [];
    }
}