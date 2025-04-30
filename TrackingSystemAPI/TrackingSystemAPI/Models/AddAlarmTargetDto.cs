namespace TrackingSystemAPI.Models
{
    public class AddAlarmTargetDto
    {

        public int alarm_id { get; set; }

        public int machine_id { get; set; }

        public string machine_name { get; set; }

        public string expression { get; set; }

        public string logic { get; set; }
    }
}
