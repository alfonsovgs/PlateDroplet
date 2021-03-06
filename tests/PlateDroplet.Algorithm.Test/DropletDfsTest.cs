﻿using AutoMapper;
using Moq;
using PlateDroplet.Algorithm.Models;
using PlateDroplet.Algorithm.Test.Data;
using PlateDroplet.Infrastructure.Repositories;
using PlateDroplet.UI.Mapper;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlateDroplet.Algorithm.Test
{
    public class DropletDfsTest
    {
        [Theory]
        [InlineData(100, 5)]
        public async Task AddThreshold_RuleGruop_ReturnTotalNumerOfGroups_Equal_Six(int thresold, int ruleGroup)
        {
            var dropletDfs = GetDropletDfs();
            var wells = await GetWells();
            
            var arrayDataConverter = new ArrayDataConverter(GetConfiguration());
            var array = arrayDataConverter.Map(wells);

            var result = dropletDfs.DeepSearch(array, thresold, ruleGroup);
            result.TotalNumberOfGroups.ShouldBe(5);
        }

        [Theory]
        [InlineData(100, 5)]
        public async Task AddThreshold_RuleGruop_ReturnNumberWellsInLargestGroup_Equal_Six(int thresold, int ruleGroup)
        {
            var dropletDfs = GetDropletDfs();
            var wells = await GetWells();
            
            var arrayDataConverter = new ArrayDataConverter(GetConfiguration());
            var array = arrayDataConverter.Map(wells);

            var result = dropletDfs.DeepSearch(array, thresold, ruleGroup);
            result.NumberWellsInLargestGroup.ShouldBe(6);
        }

        [Theory]
        [InlineData(100, 5)]
        public async Task AddThreshold_RuleGruop_ReturnNumberOfWellsInSmallestGroup_Equal_One(int thresold, int ruleGroup)
        {
            var dropletDfs = GetDropletDfs();
            var wells = await GetWells();
            
            var arrayDataConverter = new ArrayDataConverter(GetConfiguration());
            var array = arrayDataConverter.Map(wells);

            var result = dropletDfs.DeepSearch(array, thresold, ruleGroup);
            result.NumberOfWellsInSmallestGroup.ShouldBe(2);
        }

        [Theory]
        [ClassData(typeof(DropletOneGroupTheoryData))]
        public void PlateDropletResult_Returning_One_Group(WellNode[,] data, int threshold)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);
            result.TotalNumberOfGroups.ShouldBe(1);
        }

        [Theory]
        [ClassData(typeof(DropletThreeGroupThresoldAndGroupIsOneTheoryData))]
        public void PlateDropletResult_Returning_Three_Groups(WellNode[,] data, int threshold)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);
           
            result.TotalNumberOfGroups.ShouldBe(3);
        }

        [Theory]
        [ClassData(typeof(DropletOneGroupTheoryData))]
        public void PlateDropletResult_Doesnt_Contains_Wells_In_Red(WellNode[,] data, int threshold)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);
            var elements = result.FindNodesWithColor(EColor.Red);

            elements.Count().ShouldBe(0);
        } 
        
        [Theory]
        [ClassData(typeof(PlateDropletWithThreeItemsInRedData))]
        public void PlateDropletResult_Has_Three_Wells_In_Red(WellNode[,] data, int threshold)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);
            var elements = result.FindNodesWithColor(EColor.Red);

            elements.Count().ShouldBe(5);
        }

        [Theory]
        [ClassData(typeof(PlateDropletRuleGroupTwoWithThreeItemsInRed))]
        public void PlateDropletResult_Has_Four_Wells_In_Red_And_Three_Groups(WellNode[,] data, int threshold, int ruleGroup)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, ruleGroup);
            var elements = result.FindNodesWithColor(EColor.Red);

            result.TotalNumberOfGroups.ShouldBe(2);
            result.NumberWellsInLargestGroup.ShouldBe(2);
            result.NumberOfWellsInSmallestGroup.ShouldBe(2);
            elements.Count().ShouldBe(4);
        }

        [Theory]
        [ClassData(typeof(WellNodeWithoutLegendTheoryData))]
        public void WellNode_Doesnt_Have_Legend(WellNode[,] data, int threshold)
        {
            //The rule define:
            //n = normal droplet count (count is greater than the droplet threshold value)
            //L = low droplet count (count is less than the droplet threshold value)

            //Not exist an rule like <= or >= therefore exist some cases
            //that the wellNode won't have Legend.

            //In this case the thresold == 100 then the first wellNode 
            //doesn't have a legend.

            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);

            var firstNode = result.WeelsNode[0, 0];
            firstNode.Legend.ShouldBe(string.Empty);
        }

        [Theory]
        [ClassData(typeof(WellNodeLinkedWithRightTheoryData))]
        public void Get_WellNode_Without_Node_Related(WellNode[,] data, int threshold)
        {
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);

            result.WeelsNode[0,0].Down.ShouldBeNull();
            result.WeelsNode[0,0].Left.ShouldBeNull();
            result.WeelsNode[0,0].Right.ShouldBeNull();
            result.WeelsNode[0,0].Top.ShouldBeNull();
        }

        [Theory]
        [ClassData(typeof(DropletZeroGroupButExistsGrayWellTheoryData))]
        public void Result_Hasnt_Groups_And_Exists_A_Well_Gray(WellNode[,] data, int threshold)
        {
            //To validate the rule, in this case exist and well less than thresold
            //But there is only one group.
            var dropletDfs = GetDropletDfs();
            var result = dropletDfs.DeepSearch(data, threshold, 5);

            result.TotalNumberOfGroups.ShouldBe(0);
            result.WeelsNode[0, 0].Color.ShouldBe(EColor.Gray);
        }

        private IPlateDropletConfiguration GetConfiguration(int rows = 8, int cols = 12)
        {
            var configuration = new Mock<IPlateDropletConfiguration>();

            configuration.Setup(m => m.Rows)
                .Returns(rows);

            configuration.Setup(m => m.Cols)
                .Returns(cols);

            return configuration.Object;
        }

        private IDropletDfs GetDropletDfs() => new DropletDfs();

        private IPlateDropletRepository GetRepository() => new PlateDropletRepository();

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<PlateDropletMapperProfiler>());
            return new Mapper(config);
        }

        private async Task<IEnumerable<IWell>> GetWells()
        {
            var repository = GetRepository();
            var mapper = GetMapper();

            var data = await repository.GetDroplet();
            return mapper.Map<IEnumerable<IWell>>(data.Wells);
        }
    }
}
