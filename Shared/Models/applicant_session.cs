using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mediwatch.Shared.Models
{
      [Table ("Applicant_session")]
    public class applicant_session
    {
        [Key]
        public int id {get; set;}
        public float idFormation {get; set;}
        public Guid idUser {get; set;}
        public bool confirmed {get; set;}
        public bool payed {get; set;}
        public DateTime createdAt{get; set;}
    }
}