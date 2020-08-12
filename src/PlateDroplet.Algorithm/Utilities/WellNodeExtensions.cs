using PlateDroplet.Algorithm.Models;

namespace PlateDroplet.Algorithm.Utilities
{
    public static class WellNodeExtensions
    {
        public static WellNode[,] To2D(this WellNode[] wellNodes, int rows, int cols)
        {
            var index = 0;
            var matrix2D = new WellNode[rows, cols];
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    matrix2D[row, col] = wellNodes[index];
                    index++;
                }
            }

            return matrix2D;
        }

        public static int GetRows(this WellNode[,] wellNodes) => wellNodes.GetLength(0);

        public static int GetCols(this WellNode[,] wellNodes) => wellNodes.GetLength(1);
    }
}