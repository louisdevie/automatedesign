using AutomateDesign.Client.Model.Network;
using AutomateDesign.Protos;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace TestsUnitaires.ClientNetwork
{
    public class UsersClientTests
    {
        [Fact]
        public async Task SignUpAsync_ReturnsUserId()
        {
            // Arrange
            var mockUsersClient = new Mock<Users.UsersClient>();
            var mockChannel = new Mock<ChannelWrapper>();
            mockChannel.Setup(x => x.UsersClient).Returns(mockUsersClient.Object);

            var usersClient = new UsersClient(mockChannel.Object);

            // Act
            var result = await usersClient.SignUpAsync("test@example.com", "password");

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public async Task VerifyUserAsync_CallsVerifyUserAsync()
        {
            // Arrange
            var mockUsersClient = new Mock<UsersClient>();
            var mockChannel = new Mock<ChannelWrapper>();
            mockChannel.Setup(x => x.UsersClient).Returns(mockUsersClient.Object);

            var usersClient = new UsersClient(mockChannel.Object);

            // Act
            await usersClient.VerifyUserAsync(1, 1234);

            // Assert
            mockUsersClient.Verify(x => x.VerifyUserAsync(It.IsAny<VerificationRequest>(), null, null, default), Times.Once);
        }

        // Similar tests can be created for other methods in UsersClient
    }
}
