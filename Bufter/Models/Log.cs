using System.ComponentModel.DataAnnotations;

namespace Bufter.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string IpAddress { get; set; }
        public string PcName { get; set; }
        public string UserAgent { get; set; }
        public string UniKey { get; set; }
    }
}
