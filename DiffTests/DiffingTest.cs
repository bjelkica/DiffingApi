using DiffingApi.Controllers;
using DiffingApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DiffTests
{
    [TestClass]
    public class DiffingTest
    {
        [TestMethod]
        public void Diffing_StringsOfDifferentSize()
        {
            // Testing data
            string left = "AAAAAA==";
            string right = "AAA=";
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
            DiffController controller = new DiffController();
            DiffResponse actual = controller.Diffing(diff);

            // Assert
            Assert.AreEqual(expected.DiffResultType, actual.DiffResultType);
            Assert.AreEqual(expected.Diffs, actual.Diffs);
        }

        [TestMethod]
        public void Diffing_EqualStrings()
        {
            // Testing data
            string left = "AAAAAA==";
            string right = "AAAAAA==";
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
            DiffController controller = new DiffController();
            DiffResponse actual = controller.Diffing(diff);

            // Assert
            Assert.AreEqual(expected.DiffResultType, actual.DiffResultType);
            Assert.AreEqual(expected.Diffs, actual.Diffs);
        }

        [TestMethod]
        public void Diffing_DifferentStrings()
        {
            // Testing data
            string left = "AAAAAA==";
            string right = "AQABAQ==";
            DiffData diff = new DiffData
            {
                Id = 1,
                LeftData = left,
                RightData = right
            };

            // Expected result
            List<Diff> diffs = new List<Diff>();
            diffs.Add(new Diff
            {
                Offset = 1,
                Length = 1
            });
            diffs.Add(new Diff
            {
                Offset = 3,
                Length = 1
            });
            diffs.Add(new Diff
            {
                Offset = 5,
                Length = 1
            });

            DiffResponse expected = new DiffResponse
            {
                DiffResultType = "ContentDoNotMatch",
                Diffs = diffs
            };

            // Actual result
            DiffController controller = new DiffController();
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
