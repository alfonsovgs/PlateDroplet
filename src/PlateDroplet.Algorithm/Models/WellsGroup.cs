using System.Collections.Generic;

namespace PlateDroplet.Algorithm.Models
{
    public class WellsGroup
    {
        public int Group { get; private set; }
        public int MaxNodes { get; private set; }
        public EColor Color { get; private set; }
        public IList<int> IndexNode { get; private set; }

        internal WellsGroup(int group, IList<int> indexNode)
        {
            Group = group;
            MaxNodes = indexNode.Count;
            IndexNode = indexNode;
            Color = EColor.Gray;
        }

        public static WellsGroup NewWellsGroup(int group, IList<int> indexNode) => new WellsGroup(group, indexNode);

        public void Evaluate(int ruleGroup) => Color = MaxNodes >= ruleGroup ? EColor.Red : EColor.Gray;
    }
}