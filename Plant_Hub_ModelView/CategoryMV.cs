using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class CategoryMV
    {
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public String CategoryNameAr { get; set; }
        public String Description { get; set; }
        public String DescriptionAr{ get; set; }

        [Required(AllowEmptyStrings = true)]
        public IFormFile? ImageFile { get; set; } = null;
    }
}
