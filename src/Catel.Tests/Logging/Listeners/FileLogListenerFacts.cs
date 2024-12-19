﻿namespace Catel.Tests.Logging.Listeners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Logging;
    using NUnit.Framework;

    public class FileLogListenerFacts
    {
        [TestFixture]
        public class The_FilePath_Property
        {
            [TestCase(@"myapp.log", @"{AppData}\CatenaLogic\Catel.Tests\myapp.log")]
            [TestCase(@"..\myapp.log", @"{AppData}\CatenaLogic\myapp.log")]
            [TestCase(@"c:\source\myapp.log", @"c:\source\myapp.log")]
            public void ReturnsRightFullPath(string path, string expectedPath)
            {
                var assembly = GetType().Assembly;

                var fileLogListener = new FileLogListener(path, 25000, assembly);
                fileLogListener.FilePath = path;

                var returnedPath = fileLogListener.FilePath;
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                returnedPath = returnedPath.Replace(appDataPath, "{AppData}");

                Assert.That(returnedPath, Is.EqualTo(expectedPath));
            }
        }

        [TestFixture]
        public class The_DetermineFilePath_Method
        {
            [Test]
            public async Task Automatically_Creates_Directory_Async()
            {
                var assembly = GetType().Assembly;

                var fileLogListener = new CustomFileLogListener();

                Assert.That(fileLogListener.Calls.Count, Is.Not.EqualTo(0));
            }

            private class CustomFileLogListener : FileLogListener
            {
                public bool HasCreatedDirectory { get { return Calls.Any(); } }

                public List<string> Calls { get; private set; } = new List<string>();

                protected override void CreateDirectory(string directory)
                {
                    Calls.Add(directory);
                }
            }
        }
    }
}
