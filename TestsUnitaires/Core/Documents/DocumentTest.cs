using AutomateDesign.Core.Documents;
using Xunit;
using System.Linq;

namespace TestsUnitaires.Core.Documents
{
    public class DocumentTest
    {
        [Fact]
        public void Constructor_InitializesCollections()
        {
            // Arrange & Act
            Document document = new Document();

            // Assert
            Assert.NotNull(document.States);
            Assert.NotNull(document.Events);
            Assert.NotNull(document.Transitions);
        }

        [Fact]
        public void CreateState_AddsStateToStatesCollection()
        {
            // Arrange
            Document document = new Document();
            string stateName = "State1";

            // Act
            State createdState = document.CreateState(stateName);

            // Assert
            Assert.Single(document.States);
            Assert.Equal(stateName, createdState.Name);
        }

        [Fact]
        public void CreateEnumEvent_AddsEventToEnumEventsCollection()
        {
            // Arrange
            Document document = new Document();
            string eventName = "Event1";

            // Act
            EnumEvent createdEvent = document.CreateEnumEvent(eventName);

            // Assert
            Assert.Single(document.Events);
            Assert.Equal(eventName, createdEvent.Name);
        }

        [Fact]
        public void CreateTransition_AddsTransitionToTransitionsCollection()
        {
            // Arrange
            Document document = new Document();
            State fromState = document.CreateState("FromState");
            State toState = document.CreateState("ToState");
            IEvent triggeredByEvent = document.CreateEnumEvent("Event1");

            // Act
            Transition createdTransition = document.CreateTransition(fromState, toState, triggeredByEvent);

            // Assert
            Assert.Single(document.Transitions);
            Assert.Equal(fromState, createdTransition.Start);
            Assert.Equal(toState, createdTransition.End);
            Assert.Equal(triggeredByEvent, createdTransition.TriggeredBy);
        }

        [Fact]
        public void StatesProperty_ReturnsAllStates()
        {
            // Arrange
            Document document = new Document();
            State state1 = document.CreateState("State1");
            State state2 = document.CreateState("State2");

            // Act
            var states = document.States.ToList();

            // Assert
            Assert.Equal(2, states.Count);
            Assert.Contains(state1, states);
            Assert.Contains(state2, states);
        }

        [Fact]
        public void EventsProperty_ReturnsAllEnumEvents()
        {
            // Arrange
            Document document = new Document();
            EnumEvent event1 = document.CreateEnumEvent("Event1");
            EnumEvent event2 = document.CreateEnumEvent("Event2");

            // Act
            var events = document.Events.ToList();

            // Assert
            Assert.Equal(2, events.Count);
            Assert.Contains(event1, events);
            Assert.Contains(event2, events);
        }

        [Fact]
        public void TransitionsProperty_ReturnsAllTransitions()
        {
            // Arrange
            Document document = new Document();
            State fromState = document.CreateState("FromState");
            State toState = document.CreateState("ToState");
            IEvent triggeredByEvent = document.CreateEnumEvent("Event1");
            Transition transition1 = document.CreateTransition(fromState, toState, triggeredByEvent);
            Transition transition2 = document.CreateTransition(toState, fromState, triggeredByEvent);

            // Act
            var transitions = document.Transitions.ToList();

            // Assert
            Assert.Equal(2, transitions.Count);
            Assert.Contains(transition1, transitions);
            Assert.Contains(transition2, transitions);
        }
    }
}
