using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models
{
    public class AddTagDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string tag_name { get; set; }

        public string tag_data_type { get; set; }


    }
}
