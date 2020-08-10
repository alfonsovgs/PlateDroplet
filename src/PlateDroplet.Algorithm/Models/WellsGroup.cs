using System.Collections.Generic;

namespace PlateDroplet.Algorithm.Models
{
    public class WellsGroup
    {
        public int Group { get; private set; }
        public int MaxNodes { get; private set; }
        public EColor Color { get; private set; }
        public List<int> IndexNode = new List<int>();

        public static WellsGroup FromGroup(int group) => new WellsGroup {Group = group};

        public void AddMaxNodes(int maxNodes) => MaxNodes = maxNodes;

        public void Evaluate(int wellGroup) => Color = MaxNodes >= wellGroup ? EColor.Red : EColor.Gray;
    }
}