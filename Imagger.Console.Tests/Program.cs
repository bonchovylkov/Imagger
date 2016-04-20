using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagger;
using System.Drawing;

namespace Imagger.Console.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //ApplyBiTonal
            Bitmap b = (Bitmap)Image.FromFile("test.png");
            Processor.ApplyBiTonal(ref b, (byte.MaxValue * 3) / 2, System.Drawing.Color.Red, System.Drawing.Color.White);
            b.Save("result.png");

            Bitmap br = (Bitmap)Image.FromFile("test.png");
            Processor.ApplyBrightness(ref br, 100);
            br.Save("result-br.png");



        }
    }
}
