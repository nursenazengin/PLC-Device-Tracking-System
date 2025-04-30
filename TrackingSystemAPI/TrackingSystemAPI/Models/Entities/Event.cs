using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class Event
    {

        [Key]
        public int event_id { get; set; }

        public int user_id { get; set; }

        public User User { get; set; }

        public List<EventTarget> EventTargets { get; set; }

        public List<EventLog> EventLogs { get; set; }
    }
}
