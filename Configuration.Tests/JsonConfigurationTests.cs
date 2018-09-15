﻿using System;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configuration.Tests {

    [TestClass]
    public class JsonConfigurationTests {

        public TestContext TestContext { get; set; }


        [TestMethod]
        public void Save() {
            string testFile = Path.Combine(TestContext.TestRunResultsDirectory, "test.config");
            JsonConfiguration configuration =  new JsonConfiguration(TestContext.TestRunResultsDirectory, "test.config");
            configuration.SetValue("testSection 01", "testEntry", "my Value");
            configuration.SetValue("testSection 02", "testEntry", "my Value");

            configuration.Save();
           Assert.IsTrue( File.Exists(testFile));
            TestContext.WriteLine(File.ReadAllText(testFile));
        }

        [TestMethod]
        public void Load() {
            string content = @"[{""Name"":""testSection 01"",""Items"":[{""Key"":""testEntry"",""Value"":""my Value""}]},{""Name"":""testSection 02"",""Items"":[{""Key"":""testEntry"",""Value"":""my Value""}]}]";
            string testFile = Path.Combine(TestContext.TestRunResultsDirectory, "test.config");
            File.WriteAllText(testFile, content, Encoding.UTF8);
            JsonConfiguration configuration = new JsonConfiguration(TestContext.TestRunResultsDirectory, "test.config");
            configuration.Load();

            CollectionAssert.AreEquivalent(new[] { "testSection 01", "testSection 02" }, configuration.GetSectionNames());
        }

        [TestMethod]
        //[ExpectedException(typeof(FileLoadException), "Loading failed because file is empty or does not exists.")]
        public void LoadIfFileIsEmptyNothingHappens() {
            string content = null;
            string testFile = Path.Combine(TestContext.TestRunResultsDirectory, "test.config");
            File.WriteAllText(testFile, content, Encoding.UTF8);
            JsonConfiguration configuration = new JsonConfiguration(TestContext.TestRunResultsDirectory, "test.config");
            configuration.Load();
        }
    }
}