using System;

namespace SalonAPI.Configuration
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }    
}