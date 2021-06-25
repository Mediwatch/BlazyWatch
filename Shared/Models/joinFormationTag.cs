using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mediwatch.Shared.Models
{
    [Table("Formation_Tag")]
    public class JoinFormationTag
    {
        [Key]
        public Guid id { get; set; }
        public Guid idFormation {get; set;}
        public Guid idTag {get; set;}
    }
}