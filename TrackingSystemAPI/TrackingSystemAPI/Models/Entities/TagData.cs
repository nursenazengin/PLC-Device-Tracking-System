using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    [Table("tag_datas")]
    public class TagData
    {
        [Key]
        public int tag_data_id { get; set; }

        public int tag_id { get; set; }

        public int machine_id { get; set; }

        public string? tag_name { get; set; }

        public string? machine_name { get; set; }

        public float? read_value { get; set; }

        public DateTime? recorded_at { get; set; } = DateTime.UtcNow;

        public Machine? Machine { get; set; }
        public Tag? Tag { get; set; }
    }
}
