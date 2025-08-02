using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Entities
{
    public class InventoryItem
    {
        [Key]
        public int ItemId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public required string Location { get; set; }

        // Methods.
        public void DisplayInfo()
        {
            Console.WriteLine($"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
        }
    }
}