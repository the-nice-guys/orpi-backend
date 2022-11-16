using System;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Interfaces {
    public interface ITokenValidationManager {
        public TokenValidationParameters GetTokenValidationParameters(TokenType type);
    }
}