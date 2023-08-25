using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class PostMV
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
