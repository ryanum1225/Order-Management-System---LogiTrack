using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public required string CustomerName { get; set; }
        public DateTime DatePlaced { get; set; }

        // Navigation property.
        public List<InventoryItem> Items { get; set; } = [];


        // Methods.
        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
        }

        public string GetOrderSummary()
        {
            return $"Order #{OrderId} for {CustomerName} | Items: {Items.Count()} | Placed: {DatePlaced.ToString("d")}";
        }

    }
}