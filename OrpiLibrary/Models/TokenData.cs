using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Models {
    public abstract class TokenData {
        public string Issuer { get; }
        public string Audience { get; }
        public string Signature { get; protected init; }
        public double Lifetime { get; protected init; }

        protected TokenData() {
            Issuer = "AuthenticationServer";
            Audience = "ORPIClient";
            Signature = "";
            Lifetime = 0;
        }

        public virtual TokenValidationParameters GetTokenValidationParameters() {
            return new TokenValidationParameters {
                ValidateIssuer = true, 
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Signature)),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}