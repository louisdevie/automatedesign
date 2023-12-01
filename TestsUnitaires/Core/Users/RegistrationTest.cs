using System;
using System.Net.Mail;
using Xunit;

namespace AutomateDesign.Core.Users
{
    public class RegistrationTest
    {
        [Fact]
        public void Constructor_WithVerificationCodeAndExpirationAndUser_PropertiesSetCorrectly()
        {
            // Arrange
            uint verificationCode = 1234;
            DateTime expiration = DateTime.UtcNow.AddHours(1);
            User user = new User(new MailAddress("test@example.com"), HashedPassword.FromPlain("hashedPassword"));

            // Act
            Registration registration = new Registration(verificationCode, expiration, user);

            // Assert
            Assert.Equal(verificationCode, registration.VerificationCode);
            Assert.Equal(expiration, registration.Expiration);
            Assert.Equal(user, registration.User);
        }

        [Fact]
        public void Constructor_WithUser_GeneratesVerificationCodeAndSetsExpirationAndUser()
        {
            // Arrange
            User user = new User(new MailAddress("test@example.com"), HashedPassword.FromPlain("hashedPassword"));

            // Act
            Registration registration = new Registration(user);

            // Assert
            Assert.True(registration.VerificationCode >= 0 && registration.VerificationCode <= 9999);
            Assert.True(registration.Expiration > DateTime.UtcNow);
            Assert.Equal(user, registration.User);
        }

        [Fact]
        public void Expired_BeforeExpiration_ReturnsFalse()
        {
            // Arrange
            User user = new User(new MailAddress("test@example.com"), HashedPassword.FromPlain("hashedPassword"));
            Registration registration = new Registration(user);

            // Act & Assert
            Assert.False(registration.Expired);
        }

        [Fact]
        public void Expired_AfterExpiration_ReturnsTrue()
        {
            // Arrange
            User user = new User(new MailAddress("test@example.com"), HashedPassword.FromPlain("hashedPassword"));
            Registration registration = new Registration(1234, DateTime.UtcNow.AddSeconds(-1), user);

            // Act & Assert
            Assert.True(registration.Expired);
        }

        [Fact]
        public void WithUser_ReturnsNewRegistrationWithUpdatedUser()
        {
            // Arrange
            uint verificationCode = 1234;
            DateTime expiration = DateTime.UtcNow.AddHours(1);
            User user = new User(new MailAddress("test@example.com"), HashedPassword.FromPlain("hashedPassword"));
            Registration originalRegistration = new Registration(verificationCode, expiration, user);

            // Act
            User newUser = new User(new MailAddress("newtest@example.com"), HashedPassword.FromPlain("newhashedPassword"));
            Registration newRegistration = originalRegistration.WithUser(newUser);

            // Assert
            Assert.Equal(verificationCode, newRegistration.VerificationCode);
            Assert.Equal(expiration, newRegistration.Expiration);
            Assert.Equal(newUser, newRegistration.User);
        }
    }
}
