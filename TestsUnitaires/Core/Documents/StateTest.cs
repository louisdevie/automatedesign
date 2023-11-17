using AutomateDesign.Core.Documents;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestsUnitaires.Core.Documents
{
    public class StateTests
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            Mock<Document> mockDocument = new Mock<Document>();
            int id = 1;
            string name = "State1";
            StateKind kind = StateKind.NORMAL;

            // Act
            State state = new State(mockDocument.Object, id, name, kind);

            // Assert
            Assert.Equal(id, state.Id);
            Assert.Equal(name, state.Name);
            Assert.Equal(kind, state.Kind);
        }

        [Fact]
        public void NameProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            Mock<Document> mockDocument = new Mock<Document>();
            State state = new State(mockDocument.Object, 1, "State1");

            // Act
            state.Name = "UpdatedState";

            // Assert
            Assert.Equal("UpdatedState", state.Name);
        }

        [Fact]
        public void TransitionsFromProperty_ReturnsTransitionsFromState()
        {
            // Arrange
            Mock<Document> mockDocument = new Mock<Document>();
            State state = new State(mockDocument.Object, 1, "State1");

            // Act
            IEnumerable<Transition> transitionsFrom = state.TransitionsFrom;

            // Assert
            Assert.NotNull(transitionsFrom);
            Assert.Empty(transitionsFrom);
        }

        [Fact]
        public void TransitionsToProperty_ReturnsTransitionsToState()
        {
            // Arrange
            Mock<Document> mockDocument = new Mock<Document>();
            State state = new State(mockDocument.Object, 1, "State1");

            // Act
            IEnumerable<Transition> transitionsTo = state.TransitionsTo;

            // Assert
            Assert.NotNull(transitionsTo);
            Assert.Empty(transitionsTo);
        }
    }
}
