using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Models.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public string PlantName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(800)]
        public string Image { get; set; }

        [MaxLength(800)]
        public string CareDetails { get; set; }

        public string Season { get; set; }

        [MaxLength(800)]
        public string MedicalBenefit { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<SavePlant> SavedPlants { get; set; }

    }
}

