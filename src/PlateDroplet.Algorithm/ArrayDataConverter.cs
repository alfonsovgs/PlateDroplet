using PlateDroplet.Algorithm.Models;
using PlateDroplet.Algorithm.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace PlateDroplet.Algorithm
{
    /// <summary>
    /// This class provide us a "maping" array to use as Data as A[M,N] in the DFS
    /// </summary>
    public class ArrayDataConverter : IArrayDataConverter
    {
        private readonly IPlateDropletConfiguration _configuration;

        public ArrayDataConverter(IPlateDropletConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WellNode[,] Map(IEnumerable<IWell> wells)
        {
            return  wells.OrderBy(well => well.WellIndex)
                .Select(well => WellNode.FromData(well.WellIndex, well.DropletCount))
                .ToArray()
                .To2D(_configuration.Rows, _configuration.Cols);
        }
    }
}