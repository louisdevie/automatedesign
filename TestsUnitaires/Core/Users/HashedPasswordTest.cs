using Xunit;
using System.Linq;

namespace AutomateDesign.Core.Users
{
    public class HashedPasswordTest
    {
        [Fact]
        public void Constructor_ValidParameters_PropertiesSetCorrectly()
        {
            // Arrange
            byte[] hash = new byte[] { 0x01, 0x02, 0x03 };
            string salt = "testSalt";

            // Act
            HashedPassword hashedPassword = new HashedPassword(hash, salt);

            // Assert
            Assert.Equal(hash, hashedPassword.Hash);
            Assert.Equal(salt, hashedPassword.Salt);
        }

        [Fact]
        public void Match_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "testPassword";
            HashedPassword hashedPassword = HashedPassword.FromPlain(password);

            // Act & Assert
            Assert.True(hashedPassword.Match(password));
        }

        [Fact]
        public void Match_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string correctPassword = "testPassword";
            string incorrectPassword = "incorrectPassword";
            HashedPassword hashedPassword = HashedPassword.FromPlain(correctPassword);

            // Act & Assert
            Assert.False(hashedPassword.Match(incorrectPassword));
        }

        [Fact]
        public void FromPlain_ValidPassword_ReturnsHashedPasswordWithSalt()
        {
            // Arrange
            string password = "testPassword";

            // Act
            HashedPassword hashedPassword = HashedPassword.FromPlain(password);

            // Assert
            Assert.NotNull(hashedPassword.Hash);
            Assert.NotNull(hashedPassword.Salt);
        }
    }

}
