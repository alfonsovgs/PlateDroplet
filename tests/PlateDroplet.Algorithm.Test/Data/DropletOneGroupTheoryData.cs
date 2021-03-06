﻿using PlateDroplet.Algorithm.Models;
using System.Collections;
using System.Collections.Generic;

namespace PlateDroplet.Algorithm.Test.Data
{
    public class DropletOneGroupTheoryData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[,]
                {
                    {
                        WellNode.FromData(0, 50), WellNode.FromData(1, 1000), WellNode.FromData(2, 1000), WellNode.FromData(3, 1000)
                    },
                    {
                        WellNode.FromData(4, 50), WellNode.FromData(5, 1000), WellNode.FromData(6, 1000), WellNode.FromData(7, 1000)
                    },
                    {
                        WellNode.FromData(8, 1000), WellNode.FromData(9, 1000), WellNode.FromData(10, 1000), WellNode.FromData(11, 1000)
                    },
                    {
                        WellNode.FromData(12, 1000), WellNode.FromData(13, 1000), WellNode.FromData(14, 1000), WellNode.FromData(15, 1000)
                    },
                },
                100,
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}