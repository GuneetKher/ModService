using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModService.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
    }

}