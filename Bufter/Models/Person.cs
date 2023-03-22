using System.ComponentModel.DataAnnotations;

namespace Bufter.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double Bill { get; set; }
        public double TotalBill { get; set; }
        [Required]
        public int RoomId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
