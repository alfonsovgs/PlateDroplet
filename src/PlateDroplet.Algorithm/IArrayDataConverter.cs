using System.Collections.Generic;
using PlateDroplet.Algorithm.Models;

namespace PlateDroplet.Algorithm
{
    public interface IArrayDataConverter
    {
        WellNode[,] Map(IEnumerable<IWell> wells);
    }
}