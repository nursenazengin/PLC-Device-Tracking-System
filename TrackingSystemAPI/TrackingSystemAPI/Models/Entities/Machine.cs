using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackingSystemAPI.Models.Entities
{
    public class Machine
    {
        [Key]
        public int machine_id { get; set; }

        public string machine_name { get; set; }

        public List<MachineTag> MachineTags { get; set; }

        public List<TagData> TagDatas { get; set; }

        public List<AlarmTarget> AlarmTargets { get; set; }

        public List<EventTarget> EventTargets { get; set; }

    }
}
