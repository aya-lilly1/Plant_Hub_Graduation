using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Models.Models
{
    public class Category
    {
        public int Id { get; set; }
        public String CategoryName { get; set; }
        public String CategoryNameAr { get; set; }
        [MaxLength (500)]
        public String Description { get; set; }
        [MaxLength(500)]
        public String DescriptionAr { get; set; }
        [MaxLength(800)]
        public String  Image{ get; set; }
    }
}
