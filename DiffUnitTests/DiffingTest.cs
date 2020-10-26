using DiffingApi.Controllers;
using DiffingApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DiffUnitTests
{
    [TestClass]
    public class DiffingTest
    {
        [TestMethod]
        public void Diffing_StringsOfDifferentSize()
        {
            // Testing data
            DiffController controller = new DiffController();
            string left = controller.DecodeBase64String("AAAAAA==");
            string right = controller.DecodeBase64String("AAA=");
            DiffData diff = new DiffData
            {
                Id = 1,
                LeftData = left,
                RightData = right
            };

            // Expected result
            DiffResponse expected = new DiffResponse
            {
                DiffResultType = "SizeDoNotMatch",
                Diffs = null
            };

            // Actual result
            DiffResponse actual = controller.Diffing(diff);

            // Assert
            Assert.AreEqual(expected.DiffResultType, actual.DiffResultType);
            Assert.AreEqual(expected.Diffs, actual.Diffs);
        }

        [TestMethod]
        public void Diffing_EqualStrings()
        {
            // Testing data
            DiffController controller = new DiffController();
            string left = controller.DecodeBase64String("AAAAAA==");
            string right = controller.DecodeBase64String("AAAAAA==");
            DiffData diff = new DiffData
            {
                Id = 1,
                LeftData = left,
                RightData = right
            };

            // Expected result
            DiffResponse expected = new DiffResponse
            {
                DiffResultType = "Equals",
                Diffs = null
            };

            // Actual result
            DiffResponse actual = controller.Diffing(diff);

            // Assert
            Assert.AreEqual(expected.DiffResultType, actual.DiffResultType);
            Assert.AreEqual(expected.Diffs, actual.Diffs);
        }

        [TestMethod]
        public void Diffing_DifferentStrings()
        {
            // Testing data
            DiffController controller = new DiffController();
            string left = controller.DecodeBase64String("AAAAAA==");
            string right = controller.DecodeBase64String("AQABAQ==");
            DiffData diff = new DiffData
            {
                Id = 1,
                LeftData = left,
                RightData = right
            };

            // Expected result
            List<Diff> diffs = new List<Diff>
            {
                new Diff
                {
                    Offset = 0,
                    Length = 1
                },
                new Diff
                {
                    Offset = 2,
                    Length = 2
                }
            };

            DiffResponse expected = new DiffResponse
            {
                DiffResultType = "ContentDoNotMatch",
                Diffs = diffs
            };

            // Actual result
            DiffResponse actual = controller.Diffing(diff);

            // Assert
            Assert.AreEqual(expected.DiffResultType, actual.DiffResultType);
            Assert.AreEqual(expected.Diffs.Count, actual.Diffs.Count);

            int length = expected.Diffs.Count;

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(expected.Diffs[i].Length, actual.Diffs[i].Length);
                Assert.AreEqual(expected.Diffs[i].Offset, actual.Diffs[i].Offset);
            }
        }
    }
}

