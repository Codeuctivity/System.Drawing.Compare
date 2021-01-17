using System;
using System.Drawing;

namespace Codeuctivity.BitmapCompare
{
    /// <summary>
    /// Codeuctivity.BitmapCompare.Compare, inspired by testapi feature to compare images. Use this class to compare images using a third image as mask of regions where your two compared images may differ.
    /// </summary>
    public static class Compare
    {
        private const string sizeDiffersExceptionMessage = "Dimension of images differ";

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImageAreEqual(string actual, string expected)
        {
            using var actualImage = (Bitmap)Image.FromFile(actual);
            using var expectedImage = (Bitmap)Image.FromFile(expected);
            return ImageAreEqual(actualImage, expectedImage);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>True if every pixel of actual is equal to expected</returns>
        public static bool ImageAreEqual(Bitmap actual, Bitmap expected)
        {
            if (!ImagesHaveSameDimension(actual, expected))
            {
                return false;
            }

            for (var x = 0; x < actual.Width; x++)
            {
                for (var y = 0; y < actual.Height; y++)
                {
                    if (actual.GetPixel(x, y) != expected.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool ImagesHaveSameDimension(Bitmap actual, Bitmap expected)
        {
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }
            return (actual.Height == expected.Height && actual.Width == expected.Width);
        }

        /// <summary>
        /// Compares two images for equivalence using a mask Bitmap for tolerated difference between the two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <param name="pathMaskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage, string pathMaskImage)
        {
            using var actual = (Bitmap)Image.FromFile(pathActualImage);
            using var expected = (Bitmap)Image.FromFile(pathExpectedImage);
            using var mask = (Bitmap)Image.FromFile(pathMaskImage);
            return CalcDiff(actual, expected, mask);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(string pathActualImage, string pathExpectedImage)
        {
            using var actual = (Bitmap)Image.FromFile(pathActualImage);
            using var expected = (Bitmap)Image.FromFile(pathExpectedImage);
            return CalcDiff(actual, expected);
        }

        /// <summary>
        /// Compares two images for equivalence
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Bitmap actual, Bitmap expected)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
                var quantity = actual.Width * actual.Height;
                var absoluteError = 0;
                var pixelErrorCount = 0;
                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var r = Math.Abs(expected.GetPixel(x, y).R - actual.GetPixel(x, y).R);
                        var g = Math.Abs(expected.GetPixel(x, y).G - actual.GetPixel(x, y).G);
                        var b = Math.Abs(expected.GetPixel(x, y).B - actual.GetPixel(x, y).B);

                        absoluteError = absoluteError + r + g + b;

                        pixelErrorCount += (r + g + b) > 0 ? 1 : 0;
                    }
                }
                var meanError = (double)absoluteError / quantity;
                var pixelErrorPercentage = ((double)pixelErrorCount / quantity) * 100;
                return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
            }
            throw new CompareException(sizeDiffersExceptionMessage);
        }

        /// <summary>
        /// Compares two images for equivalence using a mask Bitmap for tolerated difference between the two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="maskImage"></param>
        /// <returns>Mean and absolute pixel error</returns>
        public static ICompareResult CalcDiff(Bitmap actual, Bitmap expected, Bitmap maskImage)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
                if (maskImage == null)
                {
                    throw new ArgumentNullException(nameof(maskImage));
                }

                var quantity = actual.Width * actual.Height;
                var absoluteError = 0;
                var pixelErrorCount = 0;
                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var maskImagePixel = maskImage.GetPixel(x, y);
                        var r = Math.Abs(expected.GetPixel(x, y).R - actual.GetPixel(x, y).R);
                        var g = Math.Abs(expected.GetPixel(x, y).G - actual.GetPixel(x, y).G);
                        var b = Math.Abs(expected.GetPixel(x, y).B - actual.GetPixel(x, y).B);

                        var error = 0;

                        if (r > maskImagePixel.R)
                        {
                            error += r;
                        }

                        if (g > maskImagePixel.G)
                        {
                            error += g;
                        }

                        if (b > maskImagePixel.B)
                        {
                            error += b;
                        }

                        absoluteError += error;
                        pixelErrorCount += error > 0 ? 1 : 0;
                    }
                }
                var meanError = absoluteError / quantity;
                var pixelErrorPercentage = (pixelErrorCount / quantity) * 100;
                return new CompareResult(absoluteError, meanError, pixelErrorCount, pixelErrorPercentage);
            }
            throw (new CompareException(sizeDiffersExceptionMessage));
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="pathActualImage"></param>
        /// <param name="pathExpectedImage"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Bitmap CalcDiffMaskImage(string pathActualImage, string pathExpectedImage)
        {
            using var actual = (Bitmap)Image.FromFile(pathActualImage);
            using var expected = (Bitmap)Image.FromFile(pathExpectedImage);
            return CalcDiffMaskImage(actual, expected);
        }

        /// <summary>
        /// Creates a diff mask image of two images
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns>Image representing diff, black means no diff between actual image and expected image, white means max diff</returns>
        public static Bitmap CalcDiffMaskImage(Bitmap actual, Bitmap expected)
        {
            if (ImagesHaveSameDimension(actual, expected))
            {
                var maskImage = new Bitmap(actual.Width, actual.Height);

                for (var x = 0; x < actual.Width; x++)
                {
                    for (var y = 0; y < actual.Height; y++)
                    {
                        var r = Math.Abs(expected.GetPixel(x, y).R - actual.GetPixel(x, y).R);
                        var g = Math.Abs(expected.GetPixel(x, y).G - actual.GetPixel(x, y).G);
                        var b = Math.Abs(expected.GetPixel(x, y).B - actual.GetPixel(x, y).B);
                        maskImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
                return maskImage;
            }
            throw (new CompareException(sizeDiffersExceptionMessage));
        }
    }
}