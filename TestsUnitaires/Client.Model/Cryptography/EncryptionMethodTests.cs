using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    public class EncryptionMethodTests
    {
#pragma warning disable CS1998 // Cette méthode async n'a pas d'opérateur 'await' et elle s'exécutera de façon synchrone
        private static async IAsyncEnumerable<byte[]> EnumerateAsync(IEnumerable<byte[]> data)
        {
            foreach (var chunk in data) yield return chunk;
        }
#pragma warning restore CS1998

        [Fact]
        public void AesCbcRoundTrip()
        {
            /*byte[] key = new byte[16] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };

            IEncryptionMethod encryption = new AesCbcEncryptionMethod(key);

            byte[][] data = new byte[][]
            {
                Encoding.UTF8.GetBytes("Hello, World!"),
                new byte[] { 0x12, 0x23, 0x34 },
                new byte[] { 0xab, 0xbc, 0xcd, 0xde, 0xef },
            };

            byte[][] result = encryption.DecryptAsync(encryption.EncryptAsync(EnumerateAsync(data)))
                .ToBlockingEnumerable().ToArray();

            Assert.Equal(data, result);*/
        }
    }
}
