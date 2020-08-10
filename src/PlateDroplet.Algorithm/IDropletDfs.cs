using PlateDroplet.Algorithm.Models;

namespace PlateDroplet.Algorithm
{
    public interface IDropletDfs
    {
        PlateDropletResult DeepSearch(WellNode[,] wellNodes, int threshold, int ruleGroup);
    }
}