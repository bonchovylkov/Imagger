using Imagger.Mathematics.MathObjects;
using Imagger.Operators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Imagger
{
    public class RandomJitter
    {

        public static Bitmap ApplyColorMatrix(Bitmap original, Rectangle[] objects)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new Image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the color you want ColorMatrix
            //now is set to red, but with different values 
            //you can get anything you want.
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {

                    new float[] {1f, .0f, .0f, 0, 0},
                    new float[] {1f, .0f, .0f, 0, 0},
                    new float[] {1f, .0f, .0f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });


            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            for (int z = 0; z < objects.Length; z++)
            {
                Rectangle currentRectangular = objects[z];
                //draw original image on the new image using the color matrix
                g.DrawImage(original, currentRectangular,
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);



            }

            //release sources used
            g.Dispose();
            return newBitmap;
        }

        public static void ApplyAddNoise(ref Bitmap bmp, Rectangle[] objects)
        {
            for (int z = 0; z < objects.Length; z++)
            {
                Rectangle currentRectangular = objects[z];

                Random r = new Random();


                for (int x = currentRectangular.Left; x < currentRectangular.Right; x++)
                {
                    for (int y = currentRectangular.Top; y < currentRectangular.Bottom; y++)
                    {


                        int op = r.Next(0, 2);
                        if (op == 0)
                        {
                            int num = r.Next(0, 256);

                            Color clr = bmp.GetPixel(x, y);
                            int R = (clr.R + clr.R + num) / 2;
                            if (R > 255) R = 255;
                            int G = (clr.G + clr.G + num) / 2;
                            if (G > 255) G = 255;
                            int B = (clr.B + clr.B + num) / 2;
                            if (B > 255) B = 255;
                            Color result = Color.FromArgb(255, R, G, B);
                            bmp.SetPixel(x, y, result);
                        }
                        else
                        {
                            int num = r.Next(0, 256);

                            Color clr = bmp.GetPixel(x, y);
                            int R = (clr.R + clr.R - num) / 2;
                            if (R < 0) R = 0;
                            int G = (clr.G + clr.G - num) / 2;
                            if (G < 0) G = 0;
                            int B = (clr.B + clr.B - num) / 2;
                            if (B < 0) B = 0;

                            Color result = Color.FromArgb(255, R, G, B);
                            bmp.SetPixel(x, y, result);

                        }
                    }
                }
            }
        }

        public static void ApplyRandomJitterOnRectangulars2(ref Bitmap bmp, short degree, Rectangle[] objects)
        {



            for (int z = 0; z < objects.Length; z++)
            {
                Rectangle currentRectangular = objects[z];

                //int avgR = 0, avgG = 0, avgB = 0;
                //int blurPixelCount = 1;

                //Stack<Tuple<int, int>> itemstoSet = new Stack<Tuple<int, int>>();


                for (int i = currentRectangular.Left; i < currentRectangular.Right; i++)
                {
                    for (int j = currentRectangular.Top; j < currentRectangular.Bottom; j++)
                    {
                        int calc = (int)(((SimplexNoise.Noise.Generate(i, j) + 1) / 2) * 255);
                        bmp.SetPixel(i, j, Color.FromArgb(calc, calc, calc));
                    }

                }



                //for (int i = currentRectangular.Left; i < currentRectangular.Right; i++)
                //{
                //    for (int j = currentRectangular.Top; j < currentRectangular.Bottom; j++)
                //    {
                //        Color pixel = bmp.GetPixel(i, j);
                //        avgR += pixel.R;
                //        avgG += pixel.G;
                //        avgB += pixel.B;

                //        blurPixelCount++;

                //        if (blurPixelCount % 5 == 0)
                //        {

                //            while (itemstoSet.Count > 0)
                //            {
                //               int currentAvgR = avgR / blurPixelCount;
                //               int currentAvgG = avgG / blurPixelCount;
                //               int currentAvgB = avgB / blurPixelCount;

                //                var tuple = itemstoSet.Pop();
                //                bmp.SetPixel(tuple.Item1, tuple.Item2, Color.FromArgb(currentAvgR, currentAvgG, currentAvgB));


                //            }

                //            avgR = 0;
                //            avgG = 0;
                //            avgB = 0;

                //            blurPixelCount = 1;

                //        }
                //        else
                //        {
                //            itemstoSet.Push(new Tuple<int, int>(i, j));
                //        }





                //        //Color col1 = GetRandomNearPixel(currentRectangular, bmp, i, j);// bmp.GetPixel(i + 1, j + 1);

                //        //bmp.SetPixel(i, j, col1);
                //        //Console.WriteLine(j);
                //    }

                //}

                //avgR = avgR / blurPixelCount;
                //avgG = avgG / blurPixelCount;
                //avgB = avgB / blurPixelCount;

                //for (int i = currentRectangular.Left; i < currentRectangular.Right; i++)
                //{
                //    for (int j = currentRectangular.Top; j < currentRectangular.Bottom; j++)
                //    {
                //        bmp.SetPixel(i, j, Color.FromArgb(avgR, avgG, avgB));
                //    }

                //}

            }
        }

        private static Color GetRandomNearPixel(Rectangle rec, Bitmap bmp, int i, int j)
        {
            int max = rec.Width;
            Random rnd = new Random();

            int x = rnd.Next(1, max);
            int y = rnd.Next(1, max);

            int plusOrMinus = rnd.Next(1, 3);

            int finalX = i + x;
            int finalY = j + y;

            if (plusOrMinus == 1)
            {
                finalX = i - x;
            }

            plusOrMinus = rnd.Next(1, 3);
            if (plusOrMinus == 1)
            {
                finalY = j - y;
            }

            Color col1 = bmp.GetPixel(finalX, finalY);
            return col1;
        }

        public static void ApplyRandomJitterOnRectangulars(ref Bitmap bmp, short degree, Rectangle[] objects)
        {

            for (int z = 0; z < objects.Length; z++)
            {
                Rectangle currentRectangular = objects[z];


                Bitmap TempBmp = (Bitmap)bmp.Clone();

                BitmapData bmpData = bmp.LockBits(currentRectangular, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData TempBmpData = TempBmp.LockBits(currentRectangular, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

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
        }
    }
}
