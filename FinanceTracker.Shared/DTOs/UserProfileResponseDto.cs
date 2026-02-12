using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Shared.DTOs
{
    public class UserProfileResponseDto
    {
        public string? ProfileImgUrl { get; set; } = string.Empty; 
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
