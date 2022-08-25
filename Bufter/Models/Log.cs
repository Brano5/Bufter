﻿using System.ComponentModel.DataAnnotations;

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
    }
}
