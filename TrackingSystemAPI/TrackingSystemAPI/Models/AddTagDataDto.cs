namespace TrackingSystemAPI.Models
{
    public class AddTagDataDto
    {

        public int tag_id { get; set; }

        public int machine_id { get; set; }

        public string tag_name { get; set; }

        public string machine_name { get; set; }

        public float? read_value { get; set; }

        public DateTime? recorded_at { get; set; } = DateTime.UtcNow;
    }
}
