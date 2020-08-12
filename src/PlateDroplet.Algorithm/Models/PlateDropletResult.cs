using PlateDroplet.Algorithm.Utilities;
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
            WeelsNode = MappNodes(wellNodes, wellsGroup);
            wellsGroup = wellsGroup.Where(IsEqualOrGreaterThanTwo).ToList();
            
            TotalNumberOfGroups = wellsGroup.Count;
            NumberWellsInLargestGroup = wellsGroup.Any() ? wellsGroup.Max(w => w.MaxNodes) : 0;
            NumberOfWellsInSmallestGroup = wellsGroup.Any() ? wellsGroup.Min(w => w.MaxNodes) : 0;
        }

        private WellNode[,] MappNodes(WellNode[,] wellNodes, IReadOnlyCollection<WellsGroup> wellsGroup)
        {
            var rows = wellNodes.GetRows();
            var cols = wellNodes.GetCols();

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

        private bool IsEqualOrGreaterThanTwo(WellsGroup wellsGroup) => wellsGroup.MaxNodes >= 2;

        public IEnumerable<WellNode> FindNodesWithColor(EColor color)
        {
            var rows = WeelsNode.GetRows();
            var cols = WeelsNode.GetCols();

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