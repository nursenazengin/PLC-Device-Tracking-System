using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackingSystemAPI.Models;

namespace TrackingSystemAPI.Models

{
    public class AddMachineTagDto
    {

        public int machine_id { get; set; }

        public int tag_id { get; set; }

        public string tag_name { get; set; }

        public string machine_name { get; set; }
    }
}
