using System.Collections.Generic;

namespace PlateDroplet.Infrastructure.DTOs
{
    public class DropletDto
    {
        public IEnumerable<WellDto> Wells { get; set; }
    }
}