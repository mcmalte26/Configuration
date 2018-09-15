using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configuration.Tests {
    [TestClass]
    public class ConfigurationTests {
        Configuration _configuration;

       [TestInitialize]
        public void TestInitialize() {
           _configuration = new Configuration();
            _configuration.SetValue("TestSection", "TestEntry", "TestValue");
        }

        [TestMethod]
        public void GetEntryNamesReturnsAddedSectionItemKeys() {
            Assert.IsTrue(_configuration.GetEntryNames("TestSection").Contains("TestEntry"));
        }

        [TestMethod]
        public void GetGetSectionNamesReturnsAddedSectionItemKeys() {
            Assert.IsTrue(_configuration.GetSectionNames().Contains("TestSection"));
        }


        [TestMethod]
        public void HasEntryReturnsTrue() {
            Assert.IsTrue(_configuration.HasEntry("TestSection", "TestEntry"));
        }


        [TestMethod]
        public void HasSectionReturnsTrue() {
            Assert.IsTrue(_configuration.HasSection("TestSection"));
        }


        [TestMethod]
        public void RemoveSectionRemovesTheSection() {
            _configuration.RemoveSection("TestSection");

            Assert.IsFalse(_configuration.HasSection("TestSection"));
        }


        [TestMethod]
        public void RemoveEntryRemovesTheEntry() {
            _configuration.RemoveEntry("TestSection", "TestEntry");

            Assert.IsFalse(_configuration.HasEntry("TestSection",  "TestEntry"));
        }

        [TestMethod]
        public void GetValueReturnTheValue() {
            Assert.AreEqual("TestValue", _configuration.GetValue("TestSection", "TestEntry"));
        }

    }
}
