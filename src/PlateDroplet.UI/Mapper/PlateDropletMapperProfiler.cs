using AutoMapper;
using PlateDroplet.Algorithm.Models;
using PlateDroplet.Infrastructure.DTOs;
using PlateDroplet.UI.Controls.Models;

namespace PlateDroplet.UI.Mapper
{
    public class PlateDropletMapperProfiler : Profile
    {
        public PlateDropletMapperProfiler()
        {
            CreateMap<WellDto, IWell>();
            CreateMap<IWell, WellDto>();

            CreateMap<WellNode, WellNodePanel>();
        }
    }
}