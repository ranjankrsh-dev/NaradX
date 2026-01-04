using NaradX.Domain.Entities.Auth;
using Xunit;

namespace NaradX.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void RecordFailedLogin_ShouldIncrementCounter()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };

            // Act
            user.RecordFailedLogin();

            // Assert
            Assert.Equal(1, user.FailedLoginAttempts);
            Assert.Null(user.LockoutEnd);
        }

        [Fact]
        public void RecordFailedLogin_ShouldLockout_After5Attempts()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };

            // Act
            for (int i = 0; i < 5; i++)
            {
                user.RecordFailedLogin();
            }

            // Assert
            Assert.Equal(5, user.FailedLoginAttempts);
            Assert.NotNull(user.LockoutEnd);
            Assert.True(user.LockoutEnd > DateTime.UtcNow);
            Assert.True(user.IsLockedOut);
        }

        [Fact]
        public void ResetLoginAttempts_ShouldClearCounterAndLockout()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            for (int i = 0; i < 5; i++) user.RecordFailedLogin();

            // Act
            user.ResetLoginAttempts();

            // Assert
            Assert.Equal(0, user.FailedLoginAttempts);
            Assert.Null(user.LockoutEnd);
            Assert.False(user.IsLockedOut);
        }
    }
}
