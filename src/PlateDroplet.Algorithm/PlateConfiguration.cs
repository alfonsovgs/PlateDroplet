namespace PlateDroplet.Algorithm
{
    public class PlateConfiguration : IPlateDropletConfiguration
    {
        public int Rows { get; set; }
        public int Cols { get; set; }

        public PlateConfiguration(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }
    }
}