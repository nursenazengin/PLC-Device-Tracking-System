using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class Alarm
    {
        [Key]
        public int alarm_id { get; set; }

        public int user_id { get; set; }


        public string type { get; set; }


        public User User { get; set; }

        public List<AlarmLog> AlarmLogs { get; set; }

        public List<AlarmTarget> AlarmTargets { get; set; }
    }
}
