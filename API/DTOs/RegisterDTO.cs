using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Password { get; set; }
    }
}
