using Bufter.Model;

namespace Bufter.Models
{
	public class Settings
	{
        public bool SaveRoom { get; set; }
        public bool SavePerson { get; set; }
        public string? Room { get; set; }
        public string? Person { get; set; }
        public double? Bill { get; set; }
    }
}
