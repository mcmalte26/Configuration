using Configuration.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Configuration.Tests {
    [TestClass]
    public class EncryptionTests {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetComputerId() {
            byte[] key = ComputerInfo.GetIndividualKey();
            byte[] iv = ComputerInfo.GetIndividualIV();
        }

        [TestMethod]
        public void TestMethode() {
            string testFile = Path.Combine(TestContext.TestRunResultsDirectory, "test.crypto");
            CryptoConfiguration configuration = new CryptoConfiguration(TestContext.TestRunResultsDirectory, "test.crypto");
            configuration.SetValue("testSection 01", "testEntry", "my Value");
            configuration.SetValue("testSection 02", "testEntry", "my Value");
            configuration.Save();
            Assert.IsTrue(File.Exists(testFile));
            TestContext.WriteLine(File.ReadAllText(testFile));
            configuration = new CryptoConfiguration(TestContext.TestRunResultsDirectory, "test.crypto");
            configuration.Load();
            Assert.IsTrue(configuration.HasEntry("testSection 01", "testEntry"));
        }
    }
}

