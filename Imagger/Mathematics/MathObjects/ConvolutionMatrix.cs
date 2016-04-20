using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagger.Mathematics.MathObjects
{
    public class ConvolutionMatrix
    {
        public ConvolutionMatrix()
        {
            Pixel = 1;
            Factor = 1;
        }

        public void Apply(int value)
        {
            TopLeft = TopMid = TopRight = MidLeft = MidRight = BottomLeft = BottomMid = BottomRight = Pixel = value;
        }

        public int TopLeft { get; set; }

        public int TopMid { get; set; }

        public int TopRight { get; set; }

        public int MidLeft { get; set; }

        public int MidRight { get; set; }

        public int BottomLeft { get; set; }

        public int BottomMid { get; set; }

        public int BottomRight { get; set; }

        public int Pixel { get; set; }

        public int Factor { get; set; }

        public int Offset { get; set; }
    }
}
