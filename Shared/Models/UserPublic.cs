using System;
using System.Collections.Generic;

namespace Mediwatch.Shared.Models
{    
    public class UserPublic {
        public Guid Id {get; set;}
        public string Name { get; set; }
        public string Email {get; set; }
        public string PhoneNumber {get; set;}
        public string Role {get; set;}
    }
}