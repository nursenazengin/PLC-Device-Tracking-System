using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackingSystemAPI.Models.Entities
{
    public class ReadValue
    {

        [Key]
        public int value_id { get; set; }

        public int tag_id { get; set; }

        public int tag_data_id { get; set; }

        public float? read_value { get; set; }

        [JsonIgnore]
        public virtual TagData TagData { get; set; }

        [JsonIgnore]
        public virtual Tag Tag { get; set; }
    }
}
