using System.ComponentModel.DataAnnotations;

namespace Bufter.Model
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int RoomId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
