using System;
using NUnit.Framework;
using OrionLib.Utils;

namespace OrionLib.Tests.Utils
{
    public class TimeSpanExtensionsTests
    {
        public class TestData
        {
            public TimeSpan TimeSpan;
            public string ExpectedResult;
        }

        public static TestData[] TestCases =
        {
            new TestData { TimeSpan = TimeSpan.Zero, ExpectedResult = "PT0S" },
            new TestData { TimeSpan = TimeSpan.FromSeconds(10), ExpectedResult = "PT10S" },
            new TestData { TimeSpan = TimeSpan.FromSeconds(20), ExpectedResult = "PT20S" },
            new TestData { TimeSpan = TimeSpan.FromMinutes(15), ExpectedResult = "PT15M" },
            new TestData { TimeSpan = TimeSpan.FromSeconds(130), ExpectedResult = "PT2M10S" }
        };

        [TestCaseSource(nameof(TestCases))]
        public void Can_Convert_TimeSpan_To_ISO_8601_Format(TestData testCase)
        {
            Assert.That(testCase.TimeSpan.ToIso8601(), Is.EqualTo(testCase.ExpectedResult));
        }
    }
}