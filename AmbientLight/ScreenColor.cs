using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace AmbientLight
{
    static class ScreenColor
    {
        public enum DeterminationMethod
        {
            Interpolation,
            Partly,
            None
        }

        public static BasicColor GetAverageScreenColor(DeterminationMethod method)
        {
            Bitmap screenshot = GetScreenshot();
            BasicColor avgColor = new BasicColor();
            switch (method)
            {
                case DeterminationMethod.Interpolation:
                    Bitmap bmp = new Bitmap(1, 1);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // updated: the Interpolation mode needs to be set to 
                        // HighQualityBilinear or HighQualityBicubic or this method
                        // doesn't work at all.  With either setting, the results are
                        // slightly different from the averaging method.
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(screenshot, new Rectangle(0, 0, 1, 1));
                    }
                    Color avg = bmp.GetPixel(0, 0);
                    avgColor = new BasicColor(avg.R, avg.G, avg.B);
                    break;
                case DeterminationMethod.Partly:
                    long sumR = 0, sumG = 0, sumB = 0, pxlCount = 0;
                    int stepSize = 20;

                    for (int x = 0; x < screenshot.Width; x += stepSize)
                    {
                        for (int y = 0; y < screenshot.Height; y += stepSize)
                        {
                            Color pixel = screenshot.GetPixel(x, y);
                            sumR += pixel.R;
                            sumG += pixel.G;
                            sumB += pixel.B;

                            pxlCount++;
                        }
                    }
                    avgColor = new BasicColor((byte)(sumR / pxlCount), (byte)(sumG / pxlCount), (byte)(sumB / pxlCount));
                    break;
            }

            screenshot.Dispose();
            return avgColor;
        }

        private static Bitmap GetScreenshot()
        {
            Bitmap bmpScreenshot = new Bitmap((int)GetPrimaryScreenWidth(), (int)GetPrimaryScreenHeight(), PixelFormat.Format32bppArgb);

            // graphics object from the bitmap
            using (Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot))
            {
                // take a screenshot of the entire screen
                gfxScreenshot.CopyFromScreen(0, 0, 0, 0, new Size((int)GetPrimaryScreenWidth(), (int)GetPrimaryScreenHeight()), CopyPixelOperation.SourceCopy);

            }

            return bmpScreenshot;
        }

        public static double GetPrimaryScreenWidth()
        {
            return System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        public static double GetPrimaryScreenHeight()
        {
            return System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
