using System.Collections.Generic;
using System.Linq;

namespace PlateDroplet.Algorithm.Models
{
    public class PlateDropletResult
    {
        public readonly int TotalNumberOfGroups;
        public readonly int NumberWellsInLargestGroup;
        public readonly int NumberOfWellsInSmallestGroup;

        public readonly WellNode[,] WeelsNode;

        public PlateDropletResult(WellNode[,] wellNodes, IReadOnlyCollection<WellsGroup> wellsGroup)
        {
            TotalNumberOfGroups = wellsGroup.Count;
            WeelsNode = MappNodes(wellNodes, wellsGroup);

            NumberWellsInLargestGroup = wellsGroup.Any() ? wellsGroup.Max(w => w.MaxNodes) : 0;
            NumberOfWellsInSmallestGroup = wellsGroup.Any() ? wellsGroup.Min(w => w.MaxNodes) : 0;
        }

        private WellNode[,] MappNodes(WellNode[,] wellNodes, IReadOnlyCollection<WellsGroup> wellsGroup)
        {
            var rows = wellNodes.GetLength(0);
            var cols = wellNodes.GetLength(1);

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var node = wellNodes[row, col];
                    var wellGroup = wellsGroup.FirstOrDefault(c => c.IndexNode.Contains(node.Index));

                    if (wellGroup == null) continue;

                    node.Group = wellGroup.Group;
                    node.Color = wellGroup.Color;
                }
            }

            return wellNodes;
        }

        public IEnumerable<WellNode> FindNodesWithColor(EColor color)
        {
            var rows = WeelsNode.GetLength(0);
            var cols = WeelsNode.GetLength(1);

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var node = WeelsNode[row, col];

                    if (node.Color == color)
                        yield return node;
                }
            }
        }
    }
}