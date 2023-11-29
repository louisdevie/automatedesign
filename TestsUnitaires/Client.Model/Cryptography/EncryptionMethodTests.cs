using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutomateDesign.Client.Model.Cryptography
{
    public class EncryptionMethodTests
    {
        [Fact]
        public void AesCbcRoundTrip()
        {
            DocumentChannel inputChannel = new();
            DocumentChannel middleChannel = new();
            DocumentChannel outputChannel = new();

            byte[] key = DocumentBufferTests.GenerateRandomData(16);
            IEncryptionMethod encryption = new AesCbcEncryptionMethod(key);

            byte[] header = DocumentBufferTests.GenerateRandomData(32);
            byte[] body = DocumentBufferTests.GenerateRandomData(DocumentChannel.ChunkSize + 671);

            Task encryptionTask = encryption.EncryptAsync(inputChannel.Reader, middleChannel.Writer);
            Task decryptionTask = encryption.DecryptAsync(middleChannel.Reader, outputChannel.Writer);

            inputChannel.Writer.WriteHeaderAsync(header).AsTask().Wait();
            inputChannel.Writer.WriteBodyAsync(body).AsTask().Wait();

            Task.WaitAll(encryptionTask, decryptionTask);

            Task<byte[]> headerTask = outputChannel.Reader.ReadHeaderAsync();
            headerTask.Wait();

            Assert.Equal(header, headerTask.Result);

            Task<byte[]> bodyTask = outputChannel.Reader.ReadBodyAsync();
            bodyTask.Wait();

            Assert.Equal(body, bodyTask.Result);
        }

        [Fact]
        public async void AesCbcChunkRoundTrip()
        {
            byte[] key = DocumentBufferTests.GenerateRandomData(16);
            AesCbcEncryptionMethod encryption = new(key);
            byte[] data = DocumentBufferTests.GenerateRandomData(123);

            byte[] result = await encryption.DecryptChunkAsync(await encryption.EncryptChunkAsync(data));

            Assert.Equal(data, result);

            data = DocumentBufferTests.GenerateRandomData(128);

            result = await encryption.DecryptChunkAsync(await encryption.EncryptChunkAsync(data));

            Assert.Equal(data, result);
        }
    }
}
