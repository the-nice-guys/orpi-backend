using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Models {
    public abstract class TokenData {
        public string Issuer { get; }
        public string Audience { get; }
        public string Signature { get; }
        public double Lifetime { get; }
        public TokenValidationParameters ValidationParameters { get; }

        protected TokenData(string signature, double lifetime) {
            Issuer = "AuthenticationServer";
            Audience = "ORPIClient";
            Signature = signature;
            Lifetime = lifetime;
            ValidationParameters = new TokenValidationParameters {
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