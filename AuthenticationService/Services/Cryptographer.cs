using System;
using System.Text;
using System.Security.Cryptography;
using OrpiLibrary.Interfaces;


namespace AuthenticationService.Services {
    public class Cryptographer: ICryptographer {
        public string Encrypt(string password) {
            using (var sha256 = SHA256.Create()) {
                password = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            return password;
        }
    }
}