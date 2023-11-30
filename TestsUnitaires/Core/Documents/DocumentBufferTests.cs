using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    public class DocumentBufferTests
    {
        public static byte[] GenerateRandomData(int size)
        {
            byte[] data = new byte[size];

            System.Random rng = new();
            rng.NextBytes(data);

            return data;
        }

        [Fact]
        public void SendAndReceiveOneChunk()
        {
            DocumentChannel channel = new();

            byte[] header = GenerateRandomData(39);
            byte[] body = GenerateRandomData(DocumentChannel.ChunkSize + 123);

            var headerRcvTask = channel.Reader.ReadHeaderAsync();

            Assert.False(headerRcvTask.IsCompleted);

            channel.Writer.WriteHeaderAsync(header).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(headerRcvTask.IsCompleted);
            Assert.Equal(header, headerRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderAsync(header).AsTask);

            var bodyRcvTask = channel.Reader.ReadBodyAsync();

            Assert.False(bodyRcvTask.IsCompleted);

            channel.Writer.WriteBodyAsync(body).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvTask.IsCompleted);
            Assert.Equal(body, bodyRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderAsync(header).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteBodyAsync(body).AsTask);
        }

        [Fact]
        public void SendAndReceiveMultipleChunks()
        {
            DocumentChannel channel = new();

            byte[] headerPart1 = GenerateRandomData(44);
            byte[] headerPart2 = GenerateRandomData(31);
            byte[] wholeHeader = new byte[headerPart1.Length + headerPart2.Length];
            Array.Copy(headerPart1, 0, wholeHeader, 0, headerPart1.Length);
            Array.Copy(headerPart2, 0, wholeHeader, headerPart1.Length, headerPart2.Length);

            byte[] bodyPart1 = GenerateRandomData(DocumentChannel.ChunkSize - 123);
            byte[] bodyPart2 = GenerateRandomData(456);
            byte[] bodyChunk1 = new byte[DocumentChannel.ChunkSize];
            byte[] bodyChunk2 = new byte[333];
            Array.Copy(bodyPart1, 0, bodyChunk1, 0, bodyPart1.Length);
            Array.Copy(bodyPart2, 0, bodyChunk1, bodyPart1.Length, 123);
            Array.Copy(bodyPart2, 123, bodyChunk2, 0, 333);

            var headerRcvTask = channel.Reader.ReadHeaderAsync();

            Assert.False(headerRcvTask.IsCompleted);

            channel.Writer.WriteHeaderPartAsync(headerPart1).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(headerRcvTask.IsCompleted);

            channel.Writer.WriteHeaderPartAsync(headerPart2).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(headerRcvTask.IsCompleted);

            channel.Writer.FinishWritingHeaderAsync().AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(headerRcvTask.IsCompleted);
            Assert.Equal(wholeHeader, headerRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderAsync(Array.Empty<byte>()).AsTask);

            var bodyRcvEnumerator = channel.Reader.ReadAllBodyPartsAsync().GetAsyncEnumerator();
            var bodyRcvMoveNextTask = bodyRcvEnumerator.MoveNextAsync();

            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            channel.Writer.WriteBodyPartAsync(bodyPart1).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            channel.Writer.WriteBodyPartAsync(bodyPart2).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvMoveNextTask.IsCompleted);
            Assert.Equal(bodyChunk1, bodyRcvEnumerator.Current);

            bodyRcvMoveNextTask = bodyRcvEnumerator.MoveNextAsync();
            
            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            channel.Writer.FinishWritingBodyAsync().AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvMoveNextTask.IsCompleted);
            Assert.Equal(bodyChunk2, bodyRcvEnumerator.Current);

            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteBodyPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteHeaderAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(channel.Writer.WriteBodyAsync(Array.Empty<byte>()).AsTask);
        }
    }
}
