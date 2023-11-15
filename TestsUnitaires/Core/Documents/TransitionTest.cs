using AutomateDesign.Core.Documents;
using Xunit;
using Moq;

namespace TestsUnitaires.Core.Documents
{
    public class TransitionTest
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            int id = 1;
            Mock<State> mockStartState = new Mock<State>();
            Mock<State> mockEndState = new Mock<State>();
            Mock<IEvent> mockEvent = new Mock<IEvent>();

            // Act
            Transition transition = new Transition(id, mockStartState.Object, mockEndState.Object, mockEvent.Object);

            // Assert
            Assert.Equal(id, transition.Id);
            Assert.Equal(mockStartState.Object, transition.Start);
            Assert.Equal(mockEndState.Object, transition.End);
            Assert.Equal(mockEvent.Object, transition.TriggeredBy);
        }

        [Fact]
        public void StartProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            Transition transition = new Transition(1, new State(new Document(), 1, "State1"), new State(new Document(), 2, "State2"), new Mock<IEvent>().Object);
            Mock<State> mockStartState = new Mock<State>();

            // Act
            transition.Start = mockStartState.Object;

            // Assert
            Assert.True(mockStartState.Object == transition.Start);
        }

        [Fact]
        public void EndProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            Transition transition = new Transition(1, new State(new Document(), 1, "State1"), new State(new Document(), 2, "State2"), new Mock<IEvent>().Object);
            Mock<State> mockEndState = new Mock<State>();

            // Act
            transition.End = mockEndState.Object;

            // Assert
            Assert.Equal(mockEndState.Object, transition.End);
        }

        [Fact]
        public void TriggeredByProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            Transition transition = new Transition(1, new State(new Document(), 1, "State1"), new State(new Document(), 2, "State2"), new Mock<IEvent>().Object);
            Mock<IEvent> mockEvent = new Mock<IEvent>();

            // Act
            transition.TriggeredBy = mockEvent.Object;

            // Assert
            Assert.Equal(mockEvent.Object, transition.TriggeredBy);
        }
    }
}
