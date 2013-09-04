using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dynamo;
using NUnit.Framework;

namespace Dynamo.Tests
{
    [TestFixture]
    internal class PersistentSettingsTests
    {
        [Test]
        public void CanSaveLoadSettingsShowConsole()
        {
            // Get temp test file path
            string testFilePath = System.IO.Path.GetTempPath();
            testFilePath = Path.Combine(testFilePath, "test.xml");

            // Save
            PersistentSettings setting = new PersistentSettings();
            Assert.AreEqual(false, setting.ShowConsole); // default value = false
            setting.ShowConsole = true;
            Assert.AreEqual(true, setting.ShowConsole);
            Assert.AreEqual(true, setting.Save(testFilePath));

            // Load
            PersistentSettings result = PersistentSettings.Load(testFilePath);
            Assert.AreEqual(true, result.ShowConsole);

            // Remove test file path
            File.Delete(testFilePath);
        }
    }
}