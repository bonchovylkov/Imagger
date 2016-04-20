namespace Imagger.Mathematics
{
    public struct Matrix
    {
        public int[,] _Matrix;
        public int Rows;
        public int Columns;

        public Matrix(int[,] matrix)
        {
            this._Matrix = matrix;
            this.Rows = this._Matrix.GetLength(0);
            this.Columns = this._Matrix.GetLength(1);
        }
    }
}
