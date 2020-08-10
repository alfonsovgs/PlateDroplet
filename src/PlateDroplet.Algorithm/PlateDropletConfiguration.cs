using System;

namespace PlateDroplet.Algorithm
{
    public class PlateConfiguration : IPlateDropletConfiguration
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
    }

    public class PlateDropletConfiguration
    {
        private readonly IPlateDropletConfiguration _configuration;

        public PlateDropletConfiguration(Action<IPlateDropletConfiguration> action)
        {
            _configuration = new PlateConfiguration();
            action((PlateConfiguration)_configuration);
        }

        public IPlateDropletConfiguration CreateConfiguration() => _configuration;
    }
}