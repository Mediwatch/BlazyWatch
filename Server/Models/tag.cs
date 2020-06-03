using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models 
{
    [Table ("tag")]
    public class tag {
        [Key]
        public int id { get; set; }
        public string tag_name {get; set;}
        public string description {get; set;}
    }
}