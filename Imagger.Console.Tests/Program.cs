using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagger;
using System.Drawing;
using System.Net;
using System.IO;

using System.Drawing;

using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;

namespace Imagger.Console.Tests
{
    class Program
    {
        static void Main(string[] args)
        {

            ////ApplyBiTonal
            //Bitmap b = (Bitmap)Image.FromFile("test.png");
            //Processor.ApplyBiTonal(ref b, (byte.MaxValue * 3) / 2, System.Drawing.Color.Red, System.Drawing.Color.White);
            //b.Save("result.png");


            //WebClient wc = new WebClient();
            //byte[] bytes = wc.DownloadData("https://lpportalvhds5rnqyvf495z0.blob.core.windows.net/imagger/408fcdc5-8e91-4dc9-ada8-87287c8cf77f-orinal.jpeg");
            //MemoryStream ms = new MemoryStream(bytes);
            //Bitmap imageBitmap = new Bitmap(ms);
            //Processor.ApplyBrightness(ref imageBitmap, 100);
            ////Processor.ApplyBiTonal(ref imageBitmap, (byte.MaxValue * 3) / 2, System.Drawing.Color.Red, System.Drawing.Color.White);
            //imageBitmap.Save("result.png");


            //WebClient wc = new WebClient();
            //byte[] bytes = wc.DownloadData("https://lpportalvhds5rnqyvf495z0.blob.core.windows.net/imagger/157bed84-5f10-4f43-a5e5-7f0d146e3452-orinal.jpeg");
            //MemoryStream ms = new MemoryStream(bytes);
            //Bitmap imageBitmap = new Bitmap(ms);
            //Processor.ApplyRandomJitter(ref imageBitmap, 20); //, System.Drawing.Color.Red, System.Drawing.Color.White);
            //imageBitmap.Save("result.png");

            //Bitmap br = (Bitmap)Image.FromFile("test.png");
            //Processor.ApplyBrightness(ref br, 100);
            //br.Save("result-br.png");



            #region Detect faces

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData("https://scontent-fra3-1.xx.fbcdn.net/v/t1.0-1/c0.46.200.200/1724176_10151691353752537_1722497807_n.jpg?oh=19d89af364674bd704cd38613135b4e1&oe=583097F3");
            MemoryStream ms = new MemoryStream(bytes);
            Bitmap picture = new Bitmap(ms);

            HaarObjectDetector detector;
            HaarCascade cascade = new FaceHaarCascade();
            detector = new HaarObjectDetector(cascade, 30);

            detector.SearchMode = ObjectDetectorSearchMode.NoOverlap;
            detector.ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller;
            detector.ScalingFactor = 1.5f;
            detector.UseParallelProcessing = true;

            Rectangle[] objects = detector.ProcessFrame(picture);
            // RandomJitter.ApplyRandomJitterOnRectangulars(ref picture, 20, objects);
            //   RandomJitter.ApplyRandomJitterOnRectangulars2(ref picture, 20, objects);

            RandomJitter.ApplyAddNoise(ref picture,  objects);
            picture.Save("result.png");

            //  var p = RandomJitter.ApplyColorMatrix(picture, objects);
            //p.Save("result.png");

            #endregion


        }
    }
}
