using System.ComponentModel.DataAnnotations;

namespace Bufter.Models
{
    public class BuyLog
    {
        [Key]
        public int Id { get; set; }
        public string Room { get; set; }
        public string Person { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
