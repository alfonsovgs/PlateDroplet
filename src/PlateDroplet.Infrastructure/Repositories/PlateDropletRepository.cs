using Newtonsoft.Json.Linq;
using PlateDroplet.Infrastructure.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlateDroplet.Infrastructure.Repositories
{
    public class PlateDropletRepository : IPlateDropletRepository
    {
        private const string FileName = "PlateDropletInfo.json";
        private const string Section = "PlateDropletInfo";
        private const string Child = "DropletInfo";

        public async Task<DropletDto> GetDroplet()
        {
            var path = Path.IsPathRooted(FileName)
                ? FileName
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), FileName);

            if(!File.Exists(path))
            {
                throw new ArgumentException($"File not found: {path}");
            }

            if(string.IsNullOrEmpty(Section))
            {
                throw new ArgumentException($"Section not found: {Section}");
            }

            var fileData = await File.ReadAllTextAsync(FileName);
            var allData = JObject.Parse(fileData);
            var data = allData[Section]?[Child];
            return data?.ToObject<DropletDto>();
        }
    }
}