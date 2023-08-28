using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class PlantMV
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(800)]
        public string CareDetails { get; set; }

        public string Season { get; set; }

        [MaxLength(800)]
        public string MedicalBenefit { get; set; }
        public IFormFile ImageFile { get; set; }

        public int CategoryId { get; set; }
    }
}
