using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_ModelView
{
    public class UpdatePlantMV
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string PlantNameAr { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(500)]
        public String DescriptionAr { get; set; }

        [MaxLength(800)]
        public IFormFile? ImageFile { get; set; } = null;

        [MaxLength(800)]
        public string CareDetails { get; set; }
        [MaxLength(800)]
        public string CareDetailsAr { get; set; }

        public string Season { get; set; }
        public string SeasonAr { get; set; }

        [MaxLength(800)]
        public string MedicalBenefit { get; set; }
        [MaxLength(800)]
        public string MedicalBenefitAr { get; set; }

        public int CategoryId { get; set; }
    }
}
