using AutoMapper;
using PlateDroplet.Algorithm;
using PlateDroplet.Infrastructure.Repositories;
using PlateDroplet.UI.Mapper;
using PlateDroplet.UI.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace PlateDroplet.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPlateDropletRepository, PlateDropletRepository>()
                .RegisterSingleton<IArrayDataConverter, ArrayDataConverter>()
                .RegisterSingleton<IDropletDfs, DropletDfs>()
                .RegisterInstance<IPlateDropletConfiguration>(new PlateConfiguration(rows: 8, cols: 12));

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<PlateDropletMapperProfiler>());

            var mapper = mapperConfig.CreateMapper();
            containerRegistry.RegisterInstance(mapper);
        }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();
    }
}