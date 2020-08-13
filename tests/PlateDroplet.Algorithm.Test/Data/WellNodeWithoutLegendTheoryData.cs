using System.Collections;
using System.Collections.Generic;
using PlateDroplet.Algorithm.Models;

namespace PlateDroplet.Algorithm.Test.Data
{
    public class WellNodeWithoutLegendTheoryData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[,]
                {
                    { WellNode.FromData(0, 100), WellNode.FromData(1, 50), WellNode.FromData(2, 500) },
                    { WellNode.FromData(3, 500), WellNode.FromData(4, 500), WellNode.FromData(5, 50) },
                    { WellNode.FromData(6, 500), WellNode.FromData(7, 500), WellNode.FromData(8, 50) },
                },
                100,
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}