using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class EventTarget
    {

        [Key]
        public int event_target_id { get; set; }

        public int event_id { get; set; }

        public int machine_id { get; set; }

        public string machine_name { get; set; }

        public string expression { get; set; }

        public string logic { get; set; }

        public Event Events { get; set; }

        public Machine Machine { get; set; }
    }
}
