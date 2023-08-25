using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class CategoryResMV
    {
        public int Id { get; set; }
        public String CategoryName { get; set; }
        public String Description { get; set; }
        public String Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
