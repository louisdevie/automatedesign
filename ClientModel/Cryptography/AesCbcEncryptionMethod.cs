using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    internal class AesCbcEncryptionMethod : IEncryptionMethod
    {
        private byte[] key;

        public AesCbcEncryptionMethod(byte[] key)
        {
            this.key = key;
        }

        public async IAsyncEnumerable<byte[]> DecryptAsync(IAsyncEnumerable<byte[]> data)
        {
            var enumerator = data.GetAsyncEnumerator();

            if (!await enumerator.MoveNextAsync()) yield break;
            byte[] iv = new byte[16];
            Array.Copy(enumerator.Current, 0, iv, 0, 16);
            byte padding = enumerator.Current[16];

            Aes algo = Aes.Create();
            algo.Key = this.key;
            algo.IV = iv;

            ICryptoTransform transform = algo.CreateDecryptor();
            using MemoryStream binaryStream = new();
            using CryptoStream cryptoStream = new(binaryStream, transform, CryptoStreamMode.Write);

            cryptoStream.Write(enumerator.Current, 17, enumerator.Current.Length-17);
            cryptoStream.Flush();
            byte[] chunk = new byte[enumerator.Current.Length - (17 + padding)];
            var a = binaryStream.ToArray();
            binaryStream.Seek(0, SeekOrigin.Begin);
            binaryStream.Read(chunk);
            yield return chunk;
            binaryStream.SetLength(0);
            binaryStream.Seek(0, SeekOrigin.Begin);

            while (await enumerator.MoveNextAsync())
            {
                cryptoStream.Write(enumerator.Current);
                chunk = new byte[enumerator.Current.Length];
                binaryStream.Read(chunk);
                yield return chunk;
                binaryStream.SetLength(0);
                binaryStream.Seek(0, SeekOrigin.Begin);
            }
        }

        public async IAsyncEnumerable<byte[]> EncryptAsync(IAsyncEnumerable<byte[]> data)
        {
            Aes algo = Aes.Create();
            algo.Key = this.key;

            int blockSizeInBytes = algo.BlockSize / 8;
            if (blockSizeInBytes >= 256) throw new CryptographicException("Cannot handle blocks of more than 255 bytes.");

            ICryptoTransform transform = algo.CreateEncryptor();
            using MemoryStream binaryStream = new();
            using CryptoStream cryptoStream = new(binaryStream, transform, CryptoStreamMode.Write);

            bool firstChunk = true;
            await foreach (byte[] chunk in data)
            {
                if (firstChunk)
                {
                    // on bourre avant de chiffrer pour que le premier morceau puisse être décrypté seul
                    int blocks = (int)Math.Ceiling((double)chunk.Length / blockSizeInBytes);
                    int padding = (blocks * blockSizeInBytes) - chunk.Length;

                    // ajout du vecteur d'initialisation et de la taille du bourrage au premier morceau
                    binaryStream.Write(algo.IV);
                    binaryStream.WriteByte((byte)padding);

                    cryptoStream.Write(chunk);
                    cryptoStream.Write(new byte[padding]);

                    firstChunk = false;
                }
                else
                {
                    cryptoStream.Write(chunk);
                }

                yield return binaryStream.ToArray();

                // efface le tampon en gardant la mémoire allouée
                binaryStream.SetLength(0);
                binaryStream.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
