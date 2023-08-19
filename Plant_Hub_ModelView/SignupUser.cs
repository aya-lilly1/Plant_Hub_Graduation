using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class SignupUser
    {
        public string? Id { get; set; }
        public string? Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    
     
    }
}
