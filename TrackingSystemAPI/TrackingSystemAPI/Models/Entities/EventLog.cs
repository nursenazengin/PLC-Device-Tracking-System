using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class EventLog
    {
        [Key]
        public int event_log_id { get; set; }

        public int event_id { get; set; }

        public float read_value { get; set; }

        public DateTime triggered_at { get; set; }

        public Event Event { get; set; }

    }
}
