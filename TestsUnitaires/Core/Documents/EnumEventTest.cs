using AutomateDesign.Core.Documents;
using Xunit;

namespace TestsUnitaires.Core.Documents
{
    public class EnumEventTest
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            int id = 1;
            string name = "Event1";

            // Act
            EnumEvent enumEvent = new EnumEvent(id, name);

            // Assert
            Assert.Equal(id, enumEvent.Id);
            Assert.Equal(name, enumEvent.Name);
            Assert.Equal(0, enumEvent.Order);
        }

        [Fact]
        public void NameProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            EnumEvent enumEvent = new EnumEvent(1, "Event1");
            string newName = "UpdatedEvent";

            // Act
            enumEvent.Name = newName;

            // Assert
            Assert.Equal(newName, enumEvent.Name);
        }
    }
}
