using Imagger.Mathematics.MathObjects;
using Imagger.Operators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagger
{
    //https://www.programmingalgorithms.com/algorithm/numbers-to-words
    public class Processor
    {
        /// <summary>
        /// Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        ///ApplyBiTonal(ref b, (byte.MaxValue * 3) / 2, System.Drawing.Color.Red, System.Drawing.Color.White);
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="thresholdValue"></param>
        /// <param name="upperColor"></param>
        /// <param name="lowerColor"></param>
        public static void ApplyBiTonal(ref Bitmap bmp, short thresholdValue, Color upperColor, Color lowerColor)
        {
            int MaxVal = 768;

            if (thresholdValue < 0) { thresholdValue = (short)(MaxVal / 2); }
            else if (thresholdValue > MaxVal) { thresholdValue = (short)MaxVal; }

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                int TotalRGB;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                    if (TotalRGB <= thresholdValue)
                    {
                        ptr[2] = lowerColor.R;
                        ptr[1] = lowerColor.G;
                        ptr[0] = lowerColor.B;
                    }
                    else
                    {
                        ptr[2] = upperColor.R;
                        ptr[1] = upperColor.G;
                        ptr[0] = upperColor.B;
                    }

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);

        }

        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyBrightness(ref b, 50);

        public static void ApplyBrightness(ref Bitmap bmp, byte brightnessValue)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int val = 0;

                while ((int)ptr != stopAddress)
                {
                    val = ptr[2] + brightnessValue;
                    if (val < 0) val = 0;
                    else if (val > 255) val = 255;
                    ptr[2] = (byte)val;

                    val = ptr[1] + brightnessValue;
                    if (val < 0) val = 0;
                    else if (val > 255) val = 255;
                    ptr[1] = (byte)val;

                    val = ptr[0] + brightnessValue;
                    if (val < 0) val = 0;
                    else if (val > 255) val = 255;
                    ptr[0] = (byte)val;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyColor(ref b, Color.Red);
        public static void ApplyColor(ref Bitmap bmp, System.Drawing.Color c)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int RetVal;

                while ((int)ptr != stopAddress)
                {
                    RetVal = ptr[2] + c.R;
                    if (RetVal < 0) RetVal = 0;
                    else if (RetVal > 255) RetVal = 255;
                    ptr[2] = (byte)RetVal;

                    RetVal = ptr[1] + c.G;
                    if (RetVal < 0) RetVal = 0;
                    else if (RetVal > 255) RetVal = 255;
                    ptr[1] = (byte)RetVal;

                    RetVal = ptr[0] + c.B;
                    if (RetVal < 0) RetVal = 0;
                    else if (RetVal > 255) RetVal = 255;
                    ptr[0] = (byte)RetVal;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyColorExtraction(ref b, 50, Color.Green, Color.White);
        public static void ApplyColorExtraction(ref Bitmap bmp, int threshold, System.Drawing.Color extractionColor, System.Drawing.Color otherPixelsColor)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                int ExtractionTotalRGB = extractionColor.R + extractionColor.G + extractionColor.B;
                int TotalRGB;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                    if (Math.Abs(TotalRGB - ExtractionTotalRGB) >= threshold)
                    {
                        ptr[0] = otherPixelsColor.B;
                        ptr[1] = otherPixelsColor.G;
                        ptr[2] = otherPixelsColor.R;
                    }

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyColorSubstitution(ref b, 50, Color.Green, Color.Blue);
        public static void ApplyColorSubstitution(ref Bitmap bmp, int threshold, System.Drawing.Color sourceColor, System.Drawing.Color newColor)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;
                int SrcTotalRGB = sourceColor.R + sourceColor.G + sourceColor.B;
                int TotalRGB;

                while ((int)ptr != stopAddress)
                {
                    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                    if (Math.Abs(SrcTotalRGB - TotalRGB) < threshold)
                    {
                        ptr[0] = newColor.B;
                        ptr[1] = newColor.G;
                        ptr[2] = newColor.R;
                    }

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //        Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyContrast(ref b, 30);
        public static void ApplyContrast(ref Bitmap bmp, sbyte contrastValue)
        {
            if (contrastValue < -100 || contrastValue > 100) return;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                double pixel = 0, contrast = (100.0 + contrastValue) / 100.0;

                contrast *= contrast;

                while ((int)ptr != stopAddress)
                {
                    pixel = ptr[0] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[0] = (byte)pixel;

                    pixel = ptr[1] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[1] = (byte)pixel;

                    pixel = ptr[2] / 255.0;
                    pixel -= 0.5;
                    pixel *= contrast;
                    pixel += 0.5;
                    pixel *= 255;
                    if (pixel < 0) pixel = 0;
                    else if (pixel > 255) pixel = 255;
                    ptr[2] = (byte)pixel;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }


        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyEmbossLaplacian(ref b);
        public static void ApplyEmbossLaplacian(ref Bitmap bmp)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.Apply(-1);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
            m.Pixel = 4;
            m.Offset = 127;

            Convolution C = new Convolution();
            C.Matrix = m;
            C.Convolution3x3(ref bmp);
        }

        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyBothFlip(ref b);
        public static void ApplyBothFlip(ref Bitmap bmp)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int Stride = bmpData.Stride;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;
                int i = 0, X, Y;
                int Val = 0;
                int XOffset = 0;
                int YOffset = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XOffset = BmpWidth - (X + 1);
                    YOffset = BmpHeight - (Y + 1);

                    if (XOffset < 0 && XOffset >= BmpWidth)
                        XOffset = 0;

                    if (YOffset < 0 && YOffset >= BmpHeight)
                        YOffset = 0;

                    Val = (YOffset * Stride) + (XOffset * 3);

                    ptr[0] = TempPtr[Val];
                    ptr[1] = TempPtr[Val + 1];
                    ptr[2] = TempPtr[Val + 2];

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }


        //Bitmap b = (Bitmap)Image.FromFile("rose.jpg");
        //ApplyHorizontalFlip(ref b);
        public static void ApplyHorizontalFlip(ref Bitmap bmp)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int Stride = bmpData.Stride;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;
                int i = 0, X, Y;
                int Val = 0;
                int XOffset = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XOffset = BmpWidth - (X + 1);

                    if (XOffset < 0 && XOffset >= BmpWidth)
                        XOffset = 0;

                    Val = (Y * Stride) + (XOffset * 3);

                    ptr[0] = TempPtr[Val];
                    ptr[1] = TempPtr[Val + 1];
                    ptr[2] = TempPtr[Val + 2];

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        public static void ApplyVerticalFlip(ref Bitmap bmp)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int Stride = bmpData.Stride;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;
                int i = 0, X, Y;
                int Val = 0;
                int YOffset = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    YOffset = BmpHeight - (Y + 1);

                    if (YOffset < 0 && YOffset >= BmpHeight)
                        YOffset = 0;

                    Val = (YOffset * Stride) + (X * 3);

                    ptr[0] = TempPtr[Val];
                    ptr[1] = TempPtr[Val + 1];
                    ptr[2] = TempPtr[Val + 2];

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        //ApplyGamma(ref b, 2, 1.6, 0.8);
        public static void ApplyGamma(ref Bitmap bmp, double redComponent, double greenComponent, double blueComponent)
        {
            if (redComponent < 0.2 || redComponent > 5) return;
            if (greenComponent < 0.2 || greenComponent > 5) return;
            if (blueComponent < 0.2 || blueComponent > 5) return;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    ptr[0] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[0] / 255.0, 1.0 / blueComponent)) + 0.5));
                    ptr[1] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[1] / 255.0, 1.0 / greenComponent)) + 0.5));
                    ptr[2] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[2] / 255.0, 1.0 / redComponent)) + 0.5));

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //ApplyGaussianBlur(ref b, 4);
        public static void ApplyGaussianBlur(ref Bitmap bmp, int Weight)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.Apply(1);
            m.Pixel = Weight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Factor = Weight + 12;

            Convolution C = new Convolution();
            C.Matrix = m;
            C.Convolution3x3(ref bmp);
        }

        //ApplyGrayscale(ref b);
        public static void ApplyGrayscale(ref Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    *ptr = (byte)((ptr[2] * .299) + (ptr[1] * .587) + (ptr[0] * .114));
                    ptr[1] = *ptr;
                    ptr[2] = *ptr;

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //ApplyGridPixelate(ref b, new Size(15, 15));
        public static void ApplyGridPixelate(ref Bitmap bmp, Size squareSize)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int Val = 0;
                int i = 0, X = 0, Y = 0;
                int BmpStride = bmpData.Stride;
                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int SqrWidth = squareSize.Width;
                int SqrHeight = squareSize.Height;
                int XVal = 0, YVal = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XVal = (SqrWidth - X % SqrWidth);
                    YVal = (SqrHeight - Y % SqrHeight);

                    if (XVal == SqrWidth)
                        XVal = X + -X;
                    else if (XVal > 0 && XVal < BmpWidth)
                        XVal = X + XVal;

                    if (YVal == SqrHeight)
                        YVal = Y + -Y;
                    else if (YVal > 0 && YVal < BmpHeight)
                        YVal = Y + YVal;

                    if (XVal >= 0 && XVal < BmpWidth && YVal >= 0 && YVal < BmpHeight)
                    {
                        Val = (YVal * BmpStride) + (XVal * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        //ApplyInvert(ref b);
        public static void ApplyInvert(ref Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    ptr[0] = (byte)(255 - ptr[0]);
                    ptr[1] = (byte)(255 - ptr[1]);
                    ptr[2] = (byte)(255 - ptr[2]);

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //ApplyMeanRemoval(ref b, 9);
        public static void ApplyMeanRemoval(ref Bitmap bmp, int weight)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.Apply(-1);
            m.Pixel = weight;
            m.Factor = weight - 8;

            Convolution C = new Convolution();
            C.Matrix = m;
            C.Convolution3x3(ref bmp);
        }

        //ApplyNormalPixelate(ref b, new Size(15, 15));
        public static void ApplyNormalPixelate(ref Bitmap bmp, Size squareSize)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int Val = 0;
                int i = 0, X = 0, Y = 0;
                int BmpStride = bmpData.Stride;
                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int SqrWidth = squareSize.Width;
                int SqrHeight = squareSize.Height;
                int XVal = 0, YVal = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XVal = X + (SqrWidth - X % SqrWidth);
                    YVal = Y + (SqrHeight - Y % SqrHeight);

                    if (XVal < 0 && XVal >= BmpWidth)
                        XVal = 0;

                    if (YVal < 0 && YVal >= BmpHeight)
                        YVal = 0;

                    if (XVal > 0 && XVal < BmpWidth && YVal > 0 && YVal < BmpHeight)
                    {
                        Val = (YVal * BmpStride) + (XVal * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        private static void Threshold(ref Bitmap bmp, short thresholdValue)
        {
            int MaxVal = 768;

            if (thresholdValue < 0) return;
            else if (thresholdValue > MaxVal) return;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                int TotalRGB;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                    if (TotalRGB <= thresholdValue)
                    {
                        ptr[2] = 0;
                        ptr[1] = 0;
                        ptr[0] = 0;
                    }
                    else
                    {
                        ptr[2] = 255;
                        ptr[1] = 255;
                        ptr[0] = 255;
                    }

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        private static float Px(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;

            for (i = init; i <= end; i++)
                sum += hist[i];

            return (float)sum;
        }

        private static float Mx(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;

            for (i = init; i <= end; i++)
                sum += i * hist[i];

            return (float)sum;
        }

        private static int FindMax(float[] vec, int n)
        {
            float maxVec = 0;
            int idx = 0;
            int i;

            for (i = 1; i < n - 1; i++)
            {
                if (vec[i] > maxVec)
                {
                    maxVec = vec[i];
                    idx = i;
                }
            }

            return idx;
        }

        unsafe private static void GetHistogram(byte* p, int w, int h, int ws, int[] hist)
        {
            hist.Initialize();

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w * 3; j += 3)
                {
                    int index = i * ws + j;
                    hist[p[index]]++;
                }
            }
        }

        private static int GetOtsuThreshold(Bitmap bmp)
        {
            byte t = 0;
            float[] vet = new float[256];
            int[] hist = new int[256];
            vet.Initialize();

            float p1, p2, p12;
            int k;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();

                GetHistogram(p, bmp.Width, bmp.Height, bmData.Stride, hist);

                for (k = 1; k != 255; k++)
                {
                    p1 = Px(0, k, hist);
                    p2 = Px(k + 1, 255, hist);
                    p12 = p1 * p2;
                    if (p12 == 0)
                        p12 = 1;
                    float diff = (Mx(0, k, hist) * p2) - (Mx(k + 1, 255, hist) * p1);
                    vet[k] = (float)diff * diff / p12;
                }
            }

            bmp.UnlockBits(bmData);
            t = (byte)FindMax(vet, 256);

            return t;
        }

        //ApplyOtsuThreshold(ref b);
        public static void ApplyOtsuThreshold(ref Bitmap bmp)
        {
            ApplyGrayscale(ref bmp);
            int otsuThreshold = GetOtsuThreshold(bmp) * 3;
            Threshold(ref bmp, (short)otsuThreshold);
        }

        //ApplyRandomJitter(ref b, 20);
        public static void ApplyRandomJitter(ref Bitmap bmp, short degree)
        {
            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                int BmpWidth = bmp.Width;
                int BmpHeight = bmp.Height;
                int BmpStride = bmpData.Stride;
                int i = 0, X = 0, Y = 0;
                int Val = 0, XVal = 0, YVal = 0;
                short Half = (short)(degree / 2);
                Random rand = new Random();

                while ((int)ptr != stopAddress)
                {
                    X = i % BmpWidth;
                    Y = i / BmpWidth;

                    XVal = X + (rand.Next(degree) - Half);
                    YVal = Y + (rand.Next(degree) - Half);

                    if (XVal > 0 && XVal < BmpWidth && YVal > 0 && YVal < BmpHeight)
                    {
                        Val = (YVal * BmpStride) + (XVal * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        //ApplySharpen(ref b, 11);
        public static void ApplySharpen(ref Bitmap bmp, int weight)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.Apply(0);
            m.Pixel = weight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
            m.Factor = weight - 8;

            Convolution C = new Convolution();
            C.Matrix = m;
            C.Convolution3x3(ref bmp);
        }

        //ApplySmooth(ref b, 1);
        public static void ApplySmooth(ref Bitmap bmp, int weight)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.Apply(1);
            m.Pixel = weight;
            m.Factor = weight + 8;

            Convolution C = new Convolution();
            C.Matrix = m;
            C.Convolution3x3(ref bmp);
        }

        //ApplySphere(ref b);
        public static void ApplySphere(ref Bitmap bmp)
        {
            int bmpWidth = bmp.Width;
            int bmpHeight = bmp.Height;
            int bmpStride = 0;

            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmpWidth, bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            bmpStride = bmpData.Stride;

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpStride * bmpHeight;

                int Val = 0;
                int MidX = bmpWidth / 2;
                int MidY = bmpHeight / 2;
                int i = 0, X = 0, Y = 0;
                int TrueX = 0, TrueY = 0;
                int NewX = 0, NewY = 0;

                double NewRadius = 0;
                double Theta = 0, Radius = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % bmpWidth;
                    Y = i / bmpWidth;

                    TrueX = X - MidX;
                    TrueY = Y - MidY;

                    Theta = Math.Atan2(TrueY, TrueX);
                    Radius = Math.Sqrt(TrueX * TrueX + TrueY * TrueY);
                    NewRadius = Radius * Radius / Math.Max(MidX, MidY);

                    NewX = (int)(MidX + (NewRadius * Math.Cos(Theta)));
                    NewY = (int)(MidY + (NewRadius * Math.Sin(Theta)));

                    if (!(NewY >= 0 && NewY < bmpHeight && NewX >= 0 && NewX < bmpWidth))
                        NewX = NewY = 0;

                    if (NewY >= 0 && NewY < bmpHeight && NewX >= 0 && NewX < bmpWidth)
                    {
                        Val = (NewY * bmpStride) + (NewX * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }

        //ApplyThreshold(ref b, 400);
        public static void ApplyThreshold(ref Bitmap bmp, short thresholdValue)
        {
            int MaxVal = 768;

            if (thresholdValue < 0) return;
            else if (thresholdValue > MaxVal) return;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                int TotalRGB;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                    if (TotalRGB <= thresholdValue)
                    {
                        ptr[2] = 0;
                        ptr[1] = 0;
                        ptr[0] = 0;
                    }
                    else
                    {
                        ptr[2] = 255;
                        ptr[1] = 255;
                        ptr[0] = 255;
                    }

                    ptr += 3;
                }
            }

            bmp.UnlockBits(bmpData);
        }

        //ApplyTimeWarp(ref b, 15);
        public static void ApplyTimeWarp(ref Bitmap bmp, byte factor)
        {
            int bmpWidth = bmp.Width;
            int bmpHeight = bmp.Height;
            int bmpStride = 0;

            Bitmap TempBmp = (Bitmap)bmp.Clone();

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmpWidth, bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData TempBmpData = TempBmp.LockBits(new Rectangle(0, 0, TempBmp.Width, TempBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            bmpStride = bmpData.Stride;

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* TempPtr = (byte*)TempBmpData.Scan0.ToPointer();

                int stopAddress = (int)ptr + bmpStride * bmpHeight;

                int Val = 0;
                int MidX = bmpWidth / 2;
                int MidY = bmpHeight / 2;
                int i = 0, X = 0, Y = 0;
                int TrueX = 0, TrueY = 0;
                int NewX = 0, NewY = 0;

                double NewRadius = 0;
                double Theta = 0, Radius = 0;

                while ((int)ptr != stopAddress)
                {
                    X = i % bmpWidth;
                    Y = i / bmpWidth;

                    TrueX = X - MidX;
                    TrueY = Y - MidY;

                    Theta = Math.Atan2(TrueY, TrueX);
                    Radius = Math.Sqrt(TrueX * TrueX + TrueY * TrueY);
                    NewRadius = Math.Sqrt(Radius) * factor;

                    NewX = (int)(MidX + (NewRadius * Math.Cos(Theta)));
                    NewY = (int)(MidY + (NewRadius * Math.Sin(Theta)));

                    if (NewY >= 0 && NewY < bmpHeight && NewX >= 0 && NewX < bmpWidth)
                    {
                        Val = (NewY * bmpStride) + (NewX * 3);

                        ptr[0] = TempPtr[Val];
                        ptr[1] = TempPtr[Val + 1];
                        ptr[2] = TempPtr[Val + 2];
                    }

                    ptr += 3;
                    i++;
                }
            }

            bmp.UnlockBits(bmpData);
            TempBmp.UnlockBits(TempBmpData);
        }


        public static System.Drawing.Image ScaleImageIfNeeded(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new System.Drawing.Bitmap(newWidth, newHeight);

                using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                return newImage;
            }

            return image;

        }
    }


}
