namespace TrackingSystemAPI.Models.Entities
{
    public class MachineTag
    {

        public int machine_id { get; set; }

        public int tag_id { get; set; }

        public string tag_name { get; set; }

        public string machine_name { get; set; }

        public Machine Machine { get; set; }
        public Tag Tag { get; set; }

        
    }
}
