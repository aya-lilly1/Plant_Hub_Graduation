using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class CreateCategoryMV
    {
        public String CategoryName { get; set; }
        public String CategoryNameAr { get; set; }
        [MaxLength(500)]
        public String Description { get; set; }
        [MaxLength(500)]
        public String DescriptionAr { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
