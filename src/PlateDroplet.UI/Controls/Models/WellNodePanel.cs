namespace PlateDroplet.UI.Controls.Models
{
    public class WellNodePanel
    {
        public int Index { get; set; }
        public string Legend { get; set; }
        public EColor Color { get; set; }
        public int Group { get; set; }
        public int DropletCount { get; set; }

        public string GetGroup() => Group == -1 ? "No Group" : Group.ToString();
    }
}
