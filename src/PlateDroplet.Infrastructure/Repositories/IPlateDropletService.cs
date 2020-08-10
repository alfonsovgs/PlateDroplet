using System.Threading.Tasks;
using PlateDroplet.Infrastructure.DTOs;

namespace PlateDroplet.Infrastructure.Repositories
{
    public interface IPlateDropletRepository
    {
        Task<DropletDto> GetDroplet();
    }
}