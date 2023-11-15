using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;
using System.Net.Mail;
using Xunit;

namespace TestsUnitaires.Core.Users
{
    public class UserTests
    {
        [Fact]
        public void Constructor_WithValidParameters_PropertiesSetCorrectly()
        {
            // Arrange
            int id = 1;
            MailAddress email = new MailAddress("test@example.com");
            HashedPassword password = HashedPassword.FromPlain("hashedPassword");
            bool isVerified = true;

            // Act
            User user = new User(id, email, password, isVerified);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
            Assert.Equal(isVerified, user.IsVerified);
        }

        [Fact]
        public void Constructor_WithInvalidEmail_ThrowsInvalidResourceException()
        {
            // Arrange
            int id = 1;
            string invalidEmail = "invalid_email";
            HashedPassword password = HashedPassword.FromPlain("hashedPassword");
            bool isVerified = true;

            // Act & Assert
            Assert.Throws<InvalidResourceException>(() => new User(id, invalidEmail, password, isVerified));
        }

        [Fact]
        public void WithId_ReturnsNewUserWithUpdatedId()
        {
            // Arrange
            int originalId = 1;
            MailAddress email = new MailAddress("test@example.com");
            HashedPassword password = HashedPassword.FromPlain("hashedPassword");
            bool isVerified = true;

            User originalUser = new User(originalId, email, password, isVerified);

            // Act
            int newId = 2;
            User newUser = originalUser.WithId(newId);

            // Assert
            Assert.Equal(newId, newUser.Id);
            Assert.Equal(email, newUser.Email);
            Assert.Equal(password, newUser.Password);
            Assert.Equal(isVerified, newUser.IsVerified);
        }
    }
}
