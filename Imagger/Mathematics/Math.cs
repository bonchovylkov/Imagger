using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagger.Mathematics
{
    public class Math
    {
        public static Matrix TransposeMatrix(Matrix matrix)
        {
            int[,] transpose = new int[matrix.Columns, matrix.Rows];

            for (int i = 0; i < matrix.Rows; ++i)
                for (int j = 0; j < matrix.Columns; ++j)
                    transpose[j, i] = matrix._Matrix[i, j];

            return new Matrix(transpose);
        }

        public static Matrix TransposeMatrix(int[,] matrix)
        {
            int[,] transpose = new int[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                    transpose[j, i] = matrix[i, j];

            return new Matrix(transpose);
        }

        public static int[,] TransposeArray(int[,] matrix)
        {
            int[,] transpose = new int[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                    transpose[j, i] = matrix[i, j];

            return transpose;
        }
    }
}
