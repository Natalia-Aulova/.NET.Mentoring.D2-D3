using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PasswordHashGenerator.Tests
{
    [TestClass]
    public class PasswordHashServiceTests
    {
        [TestMethod]
        public void Test_GeneratePasswordHashUsingSalt()
        {
            var password = "123456";
            var salt = new byte[] { 12, 23, 34, 45, 56, 67, 78, 89, 90, 01, 12, 23, 34, 45, 56, 67 };

            var hashService = new PasswordHashService();
            var passwordHash = hashService.GeneratePasswordHashUsingSalt(password, salt);
        }
    }
}
