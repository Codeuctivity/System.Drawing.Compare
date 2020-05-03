# System.Drawing.Compare

Compares images

Inspired by the image compare feature "Visual verification API" of [TestApi](https://blogs.msdn.microsoft.com/ivo_manolov/2009/04/20/introduction-to-testapi-part-3-visual-verification-apis/) this code supports comparing images by using a tolerance mask image. That tolerance mask image is a valid image by itself and can be manipulated.

System.Drawing.Compare focus on OS independent support and therefore depends on System.Drawing but is planned to be method semi signature and value compatible with [ImageSharp.Compare](https://github.com/Codeuctivity/ImageSharp.Compare). Expect different error values when using lossy compression formats, lossless formats result in same errors for now. System.Drawing.Compare was created because ImageSharp changed their license to AGPL.

```PowerShell
Install-Package System.Drawing.Compare
```

## Example show case

Imagine two images you want to compare, and want to accept the found difference as at state of allowed difference.

### Reference Image

![actual image](./Compare.Tests/TestData/Calc0.jpg "Refernce Image")

### Actual Image

![actual image](./Compare.Tests/TestData/Calc1.jpg "Refernce Image")

### Tolerance mask image

using "compare.CalcDiff" you can calc a diff mask from actual and reference image

Example - Create difference image

```csharp
            using var maskImage = Compare.CalcDiffMaskImage(pathPic1, pathPic2);
            maskImage.Save("differenceMask.png");
```

![differenceMask.png](./Compare.Tests/TestData/differenceMask.png "differenceMask.png")

Example - Compare two images using the created difference image. Add white pixels to  differenceMask.png where you want to allow difference.

```csharp
var maskedDiff = Compare.CalcDiff(pathPic1, pathPic2, "differenceMask.png");
Assert.That(maskedDiff.AbsoluteError, Is.EqualTo(0));
```

Now you can change differenceMask.png with your favorite editor and paint white pixels where you want to ignore difference in your image under test.