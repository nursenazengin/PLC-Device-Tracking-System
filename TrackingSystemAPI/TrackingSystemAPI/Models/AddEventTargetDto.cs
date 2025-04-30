namespace TrackingSystemAPI.Models
{
    public class AddEventTargetDto
    {

        public int event_id { get; set; }

        public int machine_id { get; set; }

        public string machine_name { get; set; }

        public string expression { get; set; }

        public string logic { get; set; }
    }
}
