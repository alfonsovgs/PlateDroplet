using PlateDroplet.Algorithm.Models;
using System.Collections;
using System.Collections.Generic;

namespace PlateDroplet.Algorithm.Test.Data
{
    public class WellNodeLinkedWithRightData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[,] {
                    {WellNode.FromData(0, 1000), WellNode.FromData(1, 50), WellNode.FromData(2, 50), },
                    {WellNode.FromData(3, 50), WellNode.FromData(4, 50), WellNode.FromData(5, 50),},
                    {WellNode.FromData(6, 50), WellNode.FromData(7, 50), WellNode.FromData(8, 50),},
                },
                100,
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}