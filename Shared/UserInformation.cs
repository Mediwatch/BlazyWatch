using System.Collections.Generic;

namespace Mediwatch.Shared
{
    public class UserInformation
    {
        public bool IsAuthenticated {get; set;}
        
        public string UserName {get; set;}    
        
        public Dictionary<string, string> Claims {get; set;}
    }
}