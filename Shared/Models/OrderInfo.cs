using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Mediwatch.Shared.Models
{
    [Table ("OrderInfo")]
    public class orderInfo
    {
        [Key]
        public long id {get; set;}
        public string invoiceId {get; set;}
        public string userId {get; set;}
        public string formationId {get; set;}
        public DateTime createAt {get; set;}
        public string billingAdress {get; set;}
        public string currency {get; set;}
        public float price {get; set;}
    }
}