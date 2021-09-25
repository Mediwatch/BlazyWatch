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
        public Guid idFormation {get; set;}
        public Guid idUser {get; set;} // idOrder for moment
        public bool confirmed {get; set;}
        public bool payed {get; set;}
        public DateTime createdAt{get; set;}
    }
}