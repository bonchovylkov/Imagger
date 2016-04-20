namespace Imagger.Mathematics
{
    using System;

    /// <summary>
    /// Statistics contains most commonly used methods simple methods
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// In statistics, the standard deviation is a measure that is used to quantify the amount of variation or dispersion of a sequence of data values.
        /// </summary>
        /// <param name="data">The sequence of which we are looking to get the standard diviation</param>
        /// <returns>The diviation value</returns>
        public static double StandardDeviation(double[] data)
        {
            double mean = 0.0;
            double sumDeviation = 0.0;
            int dataSize = data.Length;

            for (int i = 0; i < dataSize; ++i)
                mean += data[i];

            mean = mean / dataSize;

            for (int i = 0; i < dataSize; ++i)
                sumDeviation += (data[i] - mean) * (data[i] - mean);

            return System.Math.Sqrt(sumDeviation / dataSize);
        }


        /// <summary>
        /// This algorithm computes the median of the given sequence of numbers.
        /// </summary>
        /// <param name="data">The sequence of which we are looking to get the median</param>
        /// <returns>The middle object of the the sorted sequence</returns>
        public static double Median(double[] data)
        {
            Array.Sort(data);

            if (data.Length % 2 == 0)
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
            else
                return data[data.Length / 2];
        }
    }
}
