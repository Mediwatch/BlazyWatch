using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class applicant_session
    {
        [Key]
        public int id {get; set;}
        public int idFormation {get; set;}
        public int idUser {get; set;}
        public bool confirmed {get; set;}
        public bool payed {get; set;}
        public DateTime createdAt{get; set;}
    }
}