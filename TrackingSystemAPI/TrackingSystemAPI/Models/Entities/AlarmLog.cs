using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class AlarmLog
    {

        [Key]
        public int alarm_log_id { get; set; }

        public int alarm_id { get; set; }

        public float read_value { get; set; }

        public DateTime triggered_at { get; set; }

        public Alarm Alarm { get; set; }


    }
}
