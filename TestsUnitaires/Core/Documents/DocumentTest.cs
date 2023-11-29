using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsUnitaires.Core.Documents
{
    public class DocumentTest
    {
        [Fact] 
        public void TestConstructor()
        {
            Document doc = new Document();
            Assert.NotNull(doc.Header);
            Assert.NotNull(doc.States);
            Assert.NotNull(doc.Events);
            Assert.NotNull(doc.Transitions);

            Document doc2 = new Document(new DocumentHeader("TestUnitaire"));
            Assert.NotNull(doc2.Header);
            Assert.Equal(doc2.Header.Name, "TestUnitaire");
        }

        [Fact]
        public void TestCreateState() 
        {
            Document doc = new Document();
            State state = doc.CreateState("Test", StateKind.Initial);
            Assert.NotNull(state);
            Assert.Equal("Test", state.Name);
            Assert.Equal(StateKind.Initial, state.Kind);
        }

        [Fact]
        public void TestSetInitialState() 
        { 
            Document doc = new Document();
            State state = doc.CreateState("Test");
            doc.SetInitialState(state);
            Assert.NotNull(doc);
            Assert.NotNull(state);
        }

        [Fact]
        public void TestCreateEnumEvent()
        {
            Document doc = new Document();
            Assert.False(doc.Events.Any());
            doc.CreateEnumEvent("Test");
            Assert.True(doc.Events.Any());
        }

        [Fact]
        public void TestCreateTransition()
        {
            Document doc = new Document();
            State state1 = doc.CreateState("TestDebut");
            State state2 = doc.CreateState("TestFin");
            EnumEvent @event = new EnumEvent(1, "Test");
            Transition trans = doc.CreateTransition(state1, state2, @event);
            Assert.NotNull(trans);
            Assert.Equal(trans.Start, state1);
            Assert.Equal(trans.End, state2);
        }
    }
}
