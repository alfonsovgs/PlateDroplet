using AutoMapper;
using Nito.Mvvm;
using PlateDroplet.Algorithm;
using PlateDroplet.Algorithm.Models;
using PlateDroplet.Infrastructure.DTOs;
using PlateDroplet.Infrastructure.Repositories;
using PlateDroplet.UI.Controls.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PlateDroplet.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IPlateDropletRepository _plateDropletRepository;
        private readonly IArrayDataConverter _arrayDataConverter;
        private readonly IDropletDfs _dropletDfs;

        private readonly IMapper _mapper;
        private DropletDto _droplet;

        public MainWindowViewModel(
            IPlateDropletRepository plateDropletRepository,
            IArrayDataConverter arrayDataConverter,
            IDropletDfs dropletDfs,
            IMapper mapper)
        {
            _plateDropletRepository = plateDropletRepository;
            _arrayDataConverter = arrayDataConverter;
            _dropletDfs = dropletDfs;
            _mapper = mapper;
            NotifyTask.Create(OnInitialize);
        }

        private WellNodePanelResult _result;

        public WellNodePanelResult Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private int? _dropletThreshold = 100;

        public int? DropletThreshold
        {
            get => _dropletThreshold;
            set => SetProperty(ref _dropletThreshold, value);
        }

        private int _totalNumber;

        public int TotalNumber
        {
            get => _totalNumber;
            set => SetProperty(ref _totalNumber, value);
        }

        private int _numberInLargestGroup;

        public int NumberInLargestGroup
        {
            get => _numberInLargestGroup;
            set => SetProperty(ref _numberInLargestGroup, value);
        }

        private int _numberInSmallestGroup;

        public int NumberInSmallestGroup
        {
            get => _numberInSmallestGroup;
            set => SetProperty(ref _numberInSmallestGroup, value);
        }

        private bool HasValues => (DropletThreshold ?? 0) > 0 && GetObjectiveGroup() > 0;

        private DelegateCommand _updateCommand;

        public DelegateCommand UpdateCommand => _updateCommand ??= new DelegateCommand(Update, () => HasValues);

        private void Update() => UpdatePlateDroplet();

        private async Task OnInitialize()
        {
            _droplet = await _plateDropletRepository.GetDroplet();
            UpdatePlateDroplet();
        }

        private void UpdatePlateDroplet()
        {
            var weels = _mapper.Map<IEnumerable<IWell>>(_droplet.Wells);

            var data = _arrayDataConverter.Map(weels);
            var result = _dropletDfs.DeepSearch(data, DropletThreshold.Value, GetObjectiveGroup());

            TotalNumber = result.TotalNumberOfGroups;
            NumberInLargestGroup = result.NumberWellsInLargestGroup;
            NumberInSmallestGroup = result.NumberOfWellsInSmallestGroup;

            var wells = _mapper.Map<WellNodePanel[,]>(result.WeelsNode);
            Result = new WellNodePanelResult(wells);
        }

        private static int GetObjectiveGroup()
        {
            ConfigurationManager.RefreshSection("appSettings");
            int.TryParse(ConfigurationManager.AppSettings["ObjectiveGroup"], out var objectiveGroup);
            return objectiveGroup;
        }
    }
}