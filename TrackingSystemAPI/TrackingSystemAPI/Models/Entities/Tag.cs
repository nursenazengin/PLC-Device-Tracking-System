using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace TrackingSystemAPI.Models.Entities
{

    [Table("tags")]
    public class Tag
    {

        [Key]
        public int tag_id { get; set; }

        public string tag_name { get; set; }

        public string tag_data_type { get; set; }

        public List<MachineTag> MachineTags { get; set; }

        public List<TagData> TagDatas { get; set; }

        [JsonIgnore]
        public List<ReadValue> ReadValues { get; set; }
    }
}
