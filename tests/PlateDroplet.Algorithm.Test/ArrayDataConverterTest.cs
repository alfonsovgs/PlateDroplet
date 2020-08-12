using AutoMapper;
using Moq;
using PlateDroplet.Algorithm.Models;
using PlateDroplet.Infrastructure.Repositories;
using PlateDroplet.UI.Mapper;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlateDroplet.Algorithm.Utilities;
using Xunit;

namespace PlateDroplet.Algorithm.Test
{
    public class ArrayDataConverterTest
    {
        private readonly Mock<IPlateDropletConfiguration> _configuration;

        public ArrayDataConverterTest()
        {
            _configuration = new Mock<IPlateDropletConfiguration>();
        }

        [Fact]
        public async Task Transform_IEnumerable_To_Array2D()
        {
            var repository = CreateRepository();
            var mapper = CreateMapper();

            var data = await repository.GetDroplet();

            _configuration.Setup(m => m.Rows)
                .Returns(8);

            _configuration.Setup(m => m.Cols)
                .Returns(12);

            var wells = mapper.Map<IEnumerable<IWell>>(data.Wells);

            var arrayDataConverter = new ArrayDataConverter(_configuration.Object);
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
            var repository = CreateRepository();
            var mapper = CreateMapper();

            var data = await repository.GetDroplet();

            _configuration.Setup(c => c.Rows)
                .Returns(n);

            _configuration.Setup(c => c.Cols)
                .Returns(m);

            var arrayDataConverter = new ArrayDataConverter(_configuration.Object);

            var wells = mapper.Map<IEnumerable<IWell>>(data.Wells);
            var array = arrayDataConverter.Map(wells);

            var newRows = array.GetRows();
            var newCols = array.GetCols();

            newRows.ShouldBe(n);
            newCols.ShouldBe(m);
        }

        public PlateDropletRepository CreateRepository() => new PlateDropletRepository();

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<PlateDropletMapperProfiler>());
            return new Mapper(config);
        }
    }
}