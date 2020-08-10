﻿using PlateDroplet.Algorithm.Models;
using System.Collections.Generic;

namespace PlateDroplet.Algorithm
{
    public class DropletDfs : IDropletDfs
    {
        private int _rows;
        private int _cols;

        public PlateDropletResult DeepSearch(WellNode[,] wellNodes, int threshold, int ruleGroup)
        {
            _rows = wellNodes.GetLength(0);
            _cols = wellNodes.GetLength(1);

            //Apply legend in all nodes
            SetLegend(wellNodes, threshold);

            var wellsGroups = new List<WellsGroup>();
            var wellGroup = 1;

            for (var row = 0; row < _rows; ++row)
            {
                for (var col = 0; col < _cols; ++col)
                {
                    var nodeFound = FindLinkedWells(wellNodes, row, col);
                    if(nodeFound == null) continue;

                    var wellsGroup = ProccessWellGroup(nodeFound, wellGroup, ruleGroup);
                    wellsGroups.Add(wellsGroup);
                    wellGroup++;
                }
            }

            return new PlateDropletResult(wellNodes, wellsGroups);
        }

        //TODO: Mapping in other class
        private void SetLegend(WellNode[,] wellNodes,  int threshold)
        {
            for (var row = 0; row < _rows; ++row)
            {
                for (var col = 0; col < _cols; ++col)
                {
                    var node = wellNodes[row, col];
                    node.ApplyLegend(threshold);
                }
            }
        }

        /// <summary>
        /// Get linked nodes for create a "group"
        /// </summary>
        private WellNode FindLinkedWells(WellNode[,] wellNodes, int row, int col)
        {
            //For terminate the recursion we apply the important rule
            //!IsValid(row, col) for prevent array overflow
            //wellNodes[row, col].Visited it means the wellNode has all relations
            //and  wellNodes[row, col].Legend != "L" to prevent only process or determinate
            //finalization of continuous recursion.
            if (!IsValid(row, col) || wellNodes[row, col].Visited || wellNodes[row, col].Legend != "L")
            {
                return null;
            }

            var node = wellNodes[row, col];

            //if wellNode was visited
            node.Visited = true;

            //In this case I implemented a aproach of DFS to find continuos elements in the well node
            //Based in the document's rules in each elements is searched
            //its nodes in the left, top, right and down
            node.Left = FindLinkedWells(wellNodes, row, col - 1);
            node.Top = FindLinkedWells(wellNodes, row - 1, col);
            node.Right = FindLinkedWells(wellNodes, row, col + 1);
            node.Down = FindLinkedWells(wellNodes, row + 1, col);
            return node;
        }

        /// <summary>
        /// Process WellNode and get and WellsGroup to representated the group and
        /// how many elements contains the group.
        /// </summary>
        private WellsGroup ProccessWellGroup(WellNode mainNode, int groupGroup, int ruleGroup)
        {
            var wellGroup = WellsGroup.FromGroup(groupGroup);
            var maxNodes = FindNodes(mainNode);

            wellGroup.AddMaxNodes(maxNodes);
            wellGroup.Evaluate(ruleGroup);
            wellGroup.Evaluate(ruleGroup);

            //And recursion of tree aplying the DFS and count the max numbers of the Tree.
            //DFS application to get max elemnts in a Group in this case this wellNode may be a Tree.
            //And return the max elements in the WellGroup.
            int FindNodes(WellNode node)
            {
                if (node == null) return 0;
                
                wellGroup.IndexNode.Add(node.Index);
                var right = FindNodes(node.Right);
                var left = FindNodes(node.Left);
                var top = FindNodes(node.Top);
                var down = FindNodes(node.Down);

                return (right + left + top + down) + 1;
            }

            return wellGroup;
        }

        private bool IsValid(int row, int col) => row >= 0 && row < _rows && col >= 0 && col < _cols;
    }
}