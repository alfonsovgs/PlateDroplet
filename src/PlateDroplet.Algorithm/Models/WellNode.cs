namespace PlateDroplet.Algorithm.Models
{
    public class WellNode
    {
        public int Index { get; private set; }
        public string Legend { get; private set; }
        public EColor Color { get;  set; }
        public int Group { get; set; }
        public int DropletCount { get; set; }
        public bool Visited { get; set; }

        public WellNode Left { get; set; }
        public WellNode Top { get; set; }
        public WellNode Right { get; set; }
        public WellNode Down { get; set; }

        internal WellNode(int index, int dropletCount)
        {
            Index = index;
            DropletCount = dropletCount;
        }

        public static WellNode FromData(int index, int dropletCount) => new WellNode(index, dropletCount);

        public void ApplyLegend(int threshold)
        {
            //The rule define:
            //n = normal droplet count (count is greater than the droplet threshold value)
            //L = low droplet count (count is less than the droplet threshold value)

            //Not exist an rule like <= or >= therefore exist some cases
            //that the wellNode won't have Legend.
            if (DropletCount > threshold) Legend = "n";
            else
            {
                Legend = DropletCount < threshold ? "L" : string.Empty;
            }
        }
    }
}