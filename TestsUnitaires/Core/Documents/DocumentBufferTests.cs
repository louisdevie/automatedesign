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
        private byte[] GenerateRandomData(int size)
        {
            byte[] data = new byte[size];

            System.Random rng = new();
            rng.NextBytes(data);

            return data;
        }

        [Fact]
        public void SendAndReceiveOneChunk()
        {
            DocumentBuffer.Sender sender;
            DocumentBuffer.Receiver receiver;
            (receiver, sender) = DocumentBuffer.CreateSpsc();

            byte[] header = GenerateRandomData(39);
            byte[] body = GenerateRandomData(DocumentBuffer.ChunkSize + 123);

            var headerRcvTask = receiver.ReadHeaderAsync();

            Assert.False(headerRcvTask.IsCompleted);

            sender.SendHeaderAsync(header).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(headerRcvTask.IsCompleted);
            Assert.Equal(header, headerRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderAsync(header).AsTask);

            var bodyRcvTask = receiver.ReadBodyAsync();

            Assert.False(bodyRcvTask.IsCompleted);

            sender.SendBodyAsync(body).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvTask.IsCompleted);
            Assert.Equal(body, bodyRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderAsync(header).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(sender.SendBodyAsync(body).AsTask);
        }

        [Fact]
        public void SendAndReceiveMultipleChunks()
        {
            DocumentBuffer.Sender sender;
            DocumentBuffer.Receiver receiver;
            (receiver, sender) = DocumentBuffer.CreateSpsc();

            byte[] headerPart1 = GenerateRandomData(44);
            byte[] headerPart2 = GenerateRandomData(31);
            byte[] wholeHeader = new byte[headerPart1.Length + headerPart2.Length];
            Array.Copy(headerPart1, 0, wholeHeader, 0, headerPart1.Length);
            Array.Copy(headerPart2, 0, wholeHeader, headerPart1.Length, headerPart2.Length);

            byte[] bodyPart1 = GenerateRandomData(DocumentBuffer.ChunkSize - 123);
            byte[] bodyPart2 = GenerateRandomData(456);
            byte[] bodyChunk1 = new byte[DocumentBuffer.ChunkSize];
            byte[] bodyChunk2 = new byte[333];
            Array.Copy(bodyPart1, 0, bodyChunk1, 0, bodyPart1.Length);
            Array.Copy(bodyPart2, 0, bodyChunk1, bodyPart1.Length, 123);
            Array.Copy(bodyPart2, 123, bodyChunk2, 0, 333);

            var headerRcvTask = receiver.ReadHeaderAsync();

            Assert.False(headerRcvTask.IsCompleted);

            sender.SendHeaderPartAsync(headerPart1).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(headerRcvTask.IsCompleted);

            sender.SendHeaderPartAsync(headerPart2).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(headerRcvTask.IsCompleted);

            sender.FinishSendingHeaderAsync().AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(headerRcvTask.IsCompleted);
            Assert.Equal(wholeHeader, headerRcvTask.Result);

            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderAsync(Array.Empty<byte>()).AsTask);

            var bodyRcvEnumerator = receiver.ReadAllBodyPartsAsync().GetAsyncEnumerator();
            var bodyRcvMoveNextTask = bodyRcvEnumerator.MoveNextAsync();

            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            sender.SendBodyPartAsync(bodyPart1).AsTask().Wait();
            Thread.Sleep(20);

            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            sender.SendBodyPartAsync(bodyPart2).AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvMoveNextTask.IsCompleted);
            Assert.Equal(bodyChunk1, bodyRcvEnumerator.Current);

            bodyRcvMoveNextTask = bodyRcvEnumerator.MoveNextAsync();
            
            Assert.False(bodyRcvMoveNextTask.IsCompleted);

            sender.FinishSendingBodyAsync().AsTask().Wait();
            Thread.Sleep(20);

            Assert.True(bodyRcvMoveNextTask.IsCompleted);
            Assert.Equal(bodyChunk2, bodyRcvEnumerator.Current);

            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(sender.SendBodyPartAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(sender.SendHeaderAsync(Array.Empty<byte>()).AsTask);
            Assert.ThrowsAsync<InvalidOperationException>(sender.SendBodyAsync(Array.Empty<byte>()).AsTask);
        }
    }
}
