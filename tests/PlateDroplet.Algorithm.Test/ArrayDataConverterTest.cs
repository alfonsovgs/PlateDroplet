using AutoMapper;
using Moq;
using PlateDroplet.Algorithm.Models;
using PlateDroplet.Algorithm.Utilities;
using PlateDroplet.Infrastructure.Repositories;
using PlateDroplet.UI.Mapper;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PlateDroplet.Algorithm.Test
{
    public class ArrayDataConverterTest
    {
        [Fact]
        public async Task Transform_IEnumerable_To_Array2D()
        {
            var configuration = GetConfigurationMock(rows: 8, cols: 12);
            var repository = CreateRepository();
            var mapper = CreateMapper();

            var data = await repository.GetDroplet();

            var wells = mapper.Map<IEnumerable<IWell>>(data.Wells);

            var arrayDataConverter = new ArrayDataConverter(configuration.Object);
            var array = arrayDataConverter.Map(wells);

            var row = array.GetRows();
            var col = array.GetCols();

            row.ShouldBe(8);
            col.ShouldBe(12);
        }

        [Theory]
        [InlineData(5,12)]
        [InlineData(4,12)]
        [InlineData(6,6)]
        public async Task Transform_IEnumerable_ToArray2D_N_M(int n, int m)
        {
            var configuration = GetConfigurationMock(n, m);
            var repository = CreateRepository();
            var mapper = CreateMapper();

            var data = await repository.GetDroplet();

            var arrayDataConverter = new ArrayDataConverter(configuration.Object);

            var wells = mapper.Map<IEnumerable<IWell>>(data.Wells);
            var array = arrayDataConverter.Map(wells);

            var newRows = array.GetRows();
            var newCols = array.GetCols();

            newRows.ShouldBe(n);
            newCols.ShouldBe(m);
        }

        private IMock<IPlateDropletConfiguration> GetConfigurationMock(int rows, int cols)
        {
            var configuration = new Mock<IPlateDropletConfiguration>();

            configuration.Setup(m => m.Rows)
                .Returns(rows);

            configuration.Setup(m => m.Cols)
                .Returns(cols);

            return configuration;
        }

        public PlateDropletRepository CreateRepository() => new PlateDropletRepository();

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<PlateDropletMapperProfiler>());
            return new Mapper(config);
        }
    }
}