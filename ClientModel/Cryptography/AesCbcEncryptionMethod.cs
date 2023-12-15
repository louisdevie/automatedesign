using AutomateDesign.Core.Documents;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Cryptography
{
    /// <summary>
    /// Fournit un moyen de (dé)chiffrer les automates avec AES en mode Cipher Block Chaining.
    /// </summary>
    public class AesCbcEncryptionMethod : IEncryptionMethod
    {
        private byte[] key;

        /// <summary>
        /// Crée un object permettant de chiffrer les automates avec AES en mode Cipher Block Chaining.
        /// </summary>
        /// <param name="key">Une clé de 128 bits à utiliser pour (dé)chiffrer les données.</param>
        public AesCbcEncryptionMethod(byte[] key)
        {
            this.key = key;
        }

        public async Task DecryptAsync(DocumentChannelReader input, DocumentChannelWriter output)
        {
            // déchiffrage de l'en-tête
            byte[] buffer = await input.ReadHeaderAsync();
            buffer = await this.DecryptChunkAsync(buffer);
            await output.WriteHeaderAsync(buffer);

            // déchiffrage de l'automate
            await foreach (byte[] chunk in input.ReadAllBodyPartsAsync())
            {
                // déchiffrage des données d'un bloc
                await output.WriteBodyPartAsync(await this.DecryptChunkAsync(chunk));
            }
            await output.FinishWritingBodyAsync();
        }

        public async Task EncryptAsync(DocumentChannelReader input, DocumentChannelWriter output)
        {
            // chiffrage de l'en-tête
            byte[] buffer = await input.ReadHeaderAsync();
            buffer = await this.EncryptChunkAsync(buffer);
            await output.WriteHeaderAsync(buffer);

            // chiffrage de l'automate
            Queue<byte> leftOver = new();
            await foreach (byte[] chunk in input.ReadAllBodyPartsAsync())
            {
                int leftOverCount = leftOver.Count;
                buffer = new byte[leftOverCount + chunk.Length];
                leftOver.CopyTo(buffer, 0);
                Array.Copy(chunk, 0, buffer, leftOverCount, chunk.Length);

                int sizeToEncrypt = Math.Min(buffer.Length, MaximumPlainChunkSize);

                // chiffrage des données qui peuvent tenir dans un bloc de document
                await output.WriteBodyPartAsync(await this.EncryptChunkAsync(buffer.AsMemory(0, sizeToEncrypt)));

                // on passe les données restantes au bloc suivant
                for (int i = sizeToEncrypt; i < buffer.Length; i++) leftOver.Enqueue(buffer[i]);
            }
            await output.FinishWritingBodyAsync();
        }

        public async Task DecryptUnstructuredAsync(ChannelReader<byte[]> input, ChannelWriter<byte[]> output)
        {
            await foreach (byte[] chunk in input.ReadAllAsync())
            {
                byte[] decrypted = await this.DecryptChunkAsync(chunk);
                await output.WriteAsync(decrypted);
            }
            output.Complete();
        }

        // le vecteur d'initialisation prends 16 octets et le bourrage prends au minimum 1 octet
        private static int MaximumPlainChunkSize => DocumentChannel.ChunkSize - 17; 

        internal async Task<byte[]> EncryptChunkAsync(ReadOnlyMemory<byte> chunk)
        {
            Aes algo = Aes.Create();
            algo.Padding = PaddingMode.PKCS7;
            algo.Key = this.key;

            using MemoryStream output = new();

            // écriture du vecteur d'initialisation
            output.Write(algo.IV);

            using CryptoStream crypto = new(output, algo.CreateEncryptor(), CryptoStreamMode.Write);

            // écriture des données encryptées
            await crypto.WriteAsync(chunk);
            await crypto.FlushFinalBlockAsync();

            return output.ToArray();
        }

        internal async Task<byte[]> DecryptChunkAsync(byte[] bytes)
        {
            Aes algo = Aes.Create();
            algo.Padding = PaddingMode.PKCS7;
            algo.Key = this.key;

            byte[] buffer = new byte[bytes.Length - 16];
            byte[] data;

            using MemoryStream input = new(bytes);

            // lecture du vecteur d'initialisation
            byte[] ivBuffer = new byte[16];
            await input.ReadAsync(ivBuffer);
            algo.IV = ivBuffer;

            using (CryptoStream crypto = new(input, algo.CreateDecryptor(), CryptoStreamMode.Read))
            {
                // lecture des données décryptées
                int read = await crypto.ReadAsync(buffer);

                // lecture du dernier bloc avec bourrage
                read += await crypto.ReadAsync(buffer, read, buffer.Length - read);

                data = new byte[read];
                Array.Copy(buffer, 0, data, 0, read);
            }

            return data;
        }
    }
}
