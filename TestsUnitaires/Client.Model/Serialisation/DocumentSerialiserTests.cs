using AutomateDesign.Core.Documents;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Serialisation
{
    public class DocumentSerialiserTests
    {
        [Fact]
        public void HeaderRoundTrip()
        {
            Channel<byte[]> channel = Channel.CreateUnbounded<byte[]>();
            ChannelReader<byte[]> reader = channel.Reader;
            ChannelWriter<byte[]> writer = channel.Writer;
            JsonDocumentSerialiser serialiser = new();

            List<DocumentHeader> headers = new() {
                new DocumentHeader(1, "Document test 1", new DateTime(2023, 11, 24, 13, 16, 00)),
                new DocumentHeader(2, "Document test 2", new DateTime(2023, 11, 24, 13, 17, 00)),
                new DocumentHeader(3, "Document test 3", new DateTime(2023, 11, 24, 13, 18, 00)),
            };

            List<DocumentHeader> results = new();

            Task serialiseTask = Task.Run(async () =>
            {
                foreach (var header in headers)
                {
                    DocumentChannel channel = new();
                    await serialiser.SerialiseHeaderAsync(header, channel.Writer);
                    await writer.WriteAsync(await channel.Reader.ReadHeaderAsync());
                }
                writer.Complete();
            });
            Task deserialiseTask = Task.Run(async () =>
            {
                await foreach (var header in serialiser.DeserialiseHeadersAsync(reader))
                {
                    results.Add(header);
                }
            });

            Task.WaitAll(serialiseTask, deserialiseTask);

            foreach((DocumentHeader original, DocumentHeader result) in headers.Zip(results))
            {
                Assert.Equal(-1, result.Id); // l'identifiant est transmis/reçu séparément
                Assert.Equal(original.Name, result.Name);
                Assert.Equal(original.LastModificationdate, result.LastModificationdate);
            }
        }

        [Fact]
        public void DocumentRoundTrip()
        {
            DocumentChannel channel = new();
            JsonDocumentSerialiser serialiser = new();

            Document document = new Document(new DocumentHeader(0, "Document test"));
            State state1 = document.CreateState("État 1", kind: StateKind.Initial);
            State state2 = document.CreateState("État 2");
            State state3 = document.CreateState("État 3", kind: StateKind.Final);
            IEvent eventA = document.CreateEnumEvent("A");
            IEvent eventB = document.CreateEnumEvent("B");
            document.CreateTransition(state1, state2, eventA);
            document.CreateTransition(state2, state1, new DefaultEvent());
            document.CreateTransition(state2, state3, eventB);

            Task serialiseTask = serialiser.SerialiseDocumentAsync(document, channel.Writer);
            Task<Document> deserialiseTask = serialiser.DeserialiseDocumentAsync(channel.Reader);
            Task.WaitAll(serialiseTask, deserialiseTask);

            Document result = deserialiseTask.Result;

            Assert.Equal("Document test", result.Header.Name);

            State[] states = result.States.ToArray();
            Assert.Equal(3, states.Length);
            Assert.Equal("État 1", states[0].Name);
            Assert.Equal(StateKind.Initial, states[0].Kind);
            Assert.Equal("État 2", states[1].Name);
            Assert.Equal(StateKind.Normal, states[1].Kind);
            Assert.Equal("État 3", states[2].Name);
            Assert.Equal(StateKind.Final, states[2].Kind);

            EnumEvent[] events = result.Events.ToArray();
            Assert.Equal(2, events.Length);
            Assert.Equal("A", events[0].Name);
            Assert.Equal("B", events[1].Name);

            Transition[] transitions = result.Transitions.ToArray();
            Assert.Equal(3, transitions.Length);
            Assert.Same(states[0], transitions[0].Start);
            Assert.Same(states[1], transitions[0].End);
            Assert.Same(events[0], transitions[0].TriggeredBy);
            Assert.Same(states[1], transitions[1].Start);
            Assert.Same(states[0], transitions[1].End);
            Assert.IsType<DefaultEvent>(transitions[1].TriggeredBy);
            Assert.Same(states[1], transitions[2].Start);
            Assert.Same(states[2], transitions[2].End);
            Assert.Same(events[1], transitions[2].TriggeredBy);
        }
    }
}
