using PlateDroplet.Algorithm.Models;

namespace PlateDroplet.Algorithm.Utilities
{
    public static class WellNodeExtensions
    {
        public static WellNode[,] To2D(this WellNode[] wellUnits, int rows, int cols)
        {
            var index = 0;
            var matrix2D = new WellNode[rows, cols];
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    matrix2D[row, col] = wellUnits[index];
                    index++;
                }
            }

            return matrix2D;
        }
    }
}