using Codeuctivity;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace System.Drawing.CompareTestNunit
{
    public class IntegrationTest
    {
        private const string jpgCalc0 = "./TestData/Calc0.jpg";
        private const string jpgCalc1 = "./TestData/Calc1.jpg";
        private const string pngCalc0 = "./TestData/Calc0.png";
        private const string pngCalc1 = "./TestData/Calc1.png";

        [Test]
        [TestCase(jpgCalc0, jpgCalc0)]
        [TestCase(pngCalc0, pngCalc0)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            Assert.That(Compare.ImageAreEqual(pathActual, pathExpected), Is.True);
        }

        [Test]
        [TestCase(jpgCalc0, pngCalc0, 443191, 2, 153760, 95.122615129543945d, true)]
        [TestCase(jpgCalc1, pngCalc1, 441320, 2, 153802, 95.148598153967981d, true)]
        [TestCase(pngCalc1, pngCalc1, 0, 0, 0, 0, false)]
        [TestCase(jpgCalc1, jpgCalc1, 0, 0, 0, 0, false)]
        [TestCase(jpgCalc0, jpgCalc1, 208886, 1, 2094, 1.2954393605701418d, true)]
        [TestCase(pngCalc0, pngCalc1, 203027, 1, 681, 0.42129618173269651d, false)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, int expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, bool expectedValuesOsDependent)
        {
            var diff = Compare.CalcDiff(pathPic1, pathPic2);
            if (expectedValuesOsDependent && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
                Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
                Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
                Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
            }
        }

        [TestCase(pngCalc0, pngCalc1, 0, 0, 0, 0)]
        public void Diffmask(string pathPic1, string pathPic2, int expectedMeanError, int expectedAbsoluteError, int expectedPixelErrorCount, double expectedPixelErrorPercentage)
        {
            using var maskImage = Compare.CalcDiffMaskImage(pathPic1, pathPic2);
            maskImage.Save("differenceMask.png");

            var maskedDiff = Compare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
            Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
            Assert.That(maskedDiff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
            Assert.That(maskedDiff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
            Assert.That(maskedDiff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
        }

        [Test]
        [TestCase(jpgCalc0, jpgCalc1)]
        [TestCase(pngCalc0, pngCalc1)]
        [TestCase(jpgCalc0, pngCalc1)]
        [TestCase(jpgCalc0, pngCalc0)]
        [TestCase(jpgCalc1, pngCalc1)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            Assert.That(Compare.ImageAreEqual(pathActual, pathExpected), Is.False);
        }
    }
}