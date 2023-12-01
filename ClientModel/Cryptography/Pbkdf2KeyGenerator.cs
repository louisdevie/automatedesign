using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    public class Pbkdf2KeyGenerator
    {
        public byte[] GetKey(int keySize, string password, string salt)
        {
            Rfc2898DeriveBytes kdf = new(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                4096,
                HashAlgorithmName.SHA256
            );

            return kdf.GetBytes(keySize);
        }
    }
}
