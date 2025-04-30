using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class AlarmTarget
    {

        [Key]
        public int alarm_target_id { get; set; }

        public int alarm_id { get; set; }

        public int machine_id { get; set; }

        public string machine_name { get; set; }

        public string expression { get; set; }

        public string logic { get; set; }

        public Alarm Alarms { get; set; }

        public Machine Machine { get; set; }
    }
}
