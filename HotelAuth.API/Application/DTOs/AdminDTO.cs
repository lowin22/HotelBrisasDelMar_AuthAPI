using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AdminDTO
    {
        public required string UserNameDTO { get; set; }
        public required string PasswordDTO { get; set; }
    }
}
