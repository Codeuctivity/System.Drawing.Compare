using Codeuctivity.BitmapCompare;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace Codeuctivity.BitmapCompareTests
{
    public class IntegrationTest
    {
        private const string jpg0 = "../../../TestData/Calc0.jpg";
        private const string jpg1 = "../../../TestData/Calc1.jpg";
        private const string png0 = "../../../TestData/Calc0.png";
        private const string png1 = "../../../TestData/Calc1.png";
        private const string pngBlack = "../../../TestData/Black.png";
        private const string pngWhite = "../../../TestData/White.png";

        [Test]
        [TestCase(jpg0, jpg0)]
        [TestCase(png0, png0)]
        public void ShouldVerifyThatImagesAreEqual(string pathActual, string pathExpected)
        {
            Assert.That(Compare.ImageAreEqual(pathActual, pathExpected), Is.True);
        }

        [Test]
        [TestCase(jpg0, png0, 443191, 2.7417720422657199d, 153760, 95.122615129543945d, true)]
        [TestCase(jpg1, png1, 441320, 2.7301972235282475d, 153802, 95.148598153967981d, true)]
        [TestCase(png1, png1, 0, 0, 0, 0, false)]
        [TestCase(jpg1, jpg1, 0, 0, 0, 0, false)]
        [TestCase(jpg0, jpg1, 208886, 1.2922595332953899d, 2094, 1.2954393605701418d, true)]
        [TestCase(png0, png1, 203027, 1.25601321422385d, 681, 0.42129618173269651d, false)]
        [TestCase(pngBlack, pngWhite, 3060, 765, 4, 100.0d, false)]
        public void ShouldVerifyThatImagesAreSemiEqual(string pathPic1, string pathPic2, int expectedAbsoluteError, double expectedMeanError, int expectedPixelErrorCount, double expectedPixelErrorPercentage, bool expectedValuesAreOsDependent)
        {
            var diff = Compare.CalcDiff(pathPic1, pathPic2);

            if (expectedValuesAreOsDependent && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Pass();
            }
            else
            {
                Assert.That(diff.AbsoluteError, Is.EqualTo(expectedAbsoluteError), "AbsoluteError");
                Assert.That(diff.MeanError, Is.EqualTo(expectedMeanError), "MeanError");
                Assert.That(diff.PixelErrorCount, Is.EqualTo(expectedPixelErrorCount), "PixelErrorCount");
                Assert.That(diff.PixelErrorPercentage, Is.EqualTo(expectedPixelErrorPercentage), "PixelErrorPercentage");
            }
        }

        [TestCase(png0, png1, 0, 0, 0, 0)]
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
        [TestCase(jpg0, jpg1)]
        [TestCase(png0, png1)]
        [TestCase(jpg0, png1)]
        [TestCase(jpg0, png0)]
        [TestCase(jpg1, png1)]
        public void ShouldVerifyThatImagesAreNotEqual(string pathActual, string pathExpected)
        {
            Assert.That(Compare.ImageAreEqual(pathActual, pathExpected), Is.False);
        }
    }
}