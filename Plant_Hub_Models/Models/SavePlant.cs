using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Models.Models
{
    public class SavePlant
    {
        public int Id { get; set; }
        public bool status { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
