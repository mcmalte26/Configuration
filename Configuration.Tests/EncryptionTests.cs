using Configuration.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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
        public void TestCryptoConfiguration() {
            string testFile = Path.Combine(TestContext.TestRunResultsDirectory, "test.crypto");

            CryptoConfiguration configuration = new CryptoConfiguration(Path.Combine(TestContext.TestRunResultsDirectory, "test.crypto"));

            configuration.SetValue("testSection 01", "testEntry", "my Value");
            configuration.SetValue("testSection 02", "testEntry", "my Value");
            configuration.Save();

            Assert.IsTrue(File.Exists(testFile));

            configuration = new CryptoConfiguration(Path.Combine(TestContext.TestRunResultsDirectory, "test.crypto"));
            configuration.Load();

            Assert.IsTrue(configuration.HasEntry("testSection 01", "testEntry"));
        }

        [TestMethod]
        public void TestCryptoConfigurationWithPassphrase() {
            string testFile = Path.Combine(TestContext.TestDeploymentDir, "test.crypto.txt");

            CryptoConfiguration configuration = new CryptoConfiguration(testFile);

            configuration.SetValue("testSection 01", "testEntry", "my Value");
            configuration.SetValue("testSection 02", "testEntry", "my Value");
            configuration.Save("LkSuZTs4G2Gt0Om7On0f");

            configuration = new CryptoConfiguration(testFile);
            configuration.Load("LkSuZTs4G2Gt0Om7On0f");

            Assert.IsTrue(configuration.HasEntry("testSection 01", "testEntry"));
            TestContext.AddResultFile(testFile);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void TestCryptoConfigurationWithDifferentPassphraseShouldFail() {
            string testFile = Path.Combine(TestContext.TestDeploymentDir, "test.crypto.txt");

            CryptoConfiguration configuration = new CryptoConfiguration(testFile);

            configuration.SetValue("testSection 01", "testEntry", "my Value");
            configuration.SetValue("testSection 02", "testEntry", "my Value");
            configuration.Save("LkSuZTs4G2Gt0Om7On0f");

            configuration = new CryptoConfiguration(testFile);
            configuration.Load("RkltsduolsOm7L0f");
            
        }
    }
}

