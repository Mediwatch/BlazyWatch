using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Table("Compagny")]
    public class compagny
    {
        [KEY]
        public int id {get; set;}
        public string compagnyName {get; set;}
        public string countryCode {get; set;}
        public List<formation> compagnyFormation {get; set;}
        public DateTime createdAt{get; set;}
    }
}
