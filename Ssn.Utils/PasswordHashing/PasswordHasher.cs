// Copyright © 2015 Stig Schmidt Nielsson. All rights reserved. Distributed under the terms of the MIT License (http://opensource.org/licenses/MIT).
using System.Linq;
using System.Security.Cryptography;
using Ssn.Utils.Extensions;
namespace Ssn.Utils.PasswordHashing {
    public class PasswordHasher {
        private readonly string _salt;

        public PasswordHasher(string salt) {
            _salt = salt;
        }

        public byte[] CreateRandomSalt(int size = 64) {
            var randomSalt = new byte[size];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomSalt);
            return randomSalt;
        }

        public byte[] HashPassword(string password, string randomSalt) {
            return HashPassword(password, randomSalt.GetBytes());
        }

        public byte[] HashPassword(string password, byte[] randomSalt) {
            var salt = _salt.GetBytes().Concat(randomSalt).ToArray();
            byte[] hashedPassword;
            using (var rfc2898 = new Rfc2898DeriveBytes(password.GetBytes(), salt, 4096)) {
                hashedPassword = rfc2898.GetBytes(32);
            }
            return hashedPassword;
        }
    }
}