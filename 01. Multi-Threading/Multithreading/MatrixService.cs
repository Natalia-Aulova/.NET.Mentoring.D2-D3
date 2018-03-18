using System;
using System.Threading.Tasks;

namespace Multithreading
{
    public class MatrixService
    {
        public int[,] Multiply(int[,] first, int[,] second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.GetLength(1) != second.GetLength(0))
            {
                throw new ArgumentException("A number of columns in the first matrix should be equal to a number of rows in the second matrix");
            }

            var resultRows = first.GetLength(0);
            var resultColumns = second.GetLength(1);
            var sharedCount = first.GetLength(1);

            var result = new int[resultRows, resultColumns];

            Parallel.For(0, resultRows, i =>
            {
                for (int j = 0; j < resultColumns; j++)
                {
                    result[i, j] = GetValue(first, second, i, j, sharedCount);
                }
            });

            return result;
        }
        
        private int GetValue(int[,] first, int[,] second, int firstIndex, int secondIndex, int count)
        {
            var sum = 0;

            for (int y = 0; y < count; y++)
            {
                sum += first[firstIndex, y] * second[y, secondIndex];
            }
            
            return sum;
        }
    }
}
