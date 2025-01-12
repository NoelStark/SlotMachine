using CommunityToolkit.Mvvm.ComponentModel;
using SlotMachine.Models;
using SlotMachine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlotMachine.ViewModels.SlotViewModels;

namespace SlotMachine.ViewModels
{
	public partial class StatisticsViewModel : ObservableObject
	{
		private GameService _gameService;
        public ObservableCollection<WinDataModel> Windata { get; set; } = new ObservableCollection<WinDataModel>
		{
			new WinDataModel { Category = "Jackpot Wins", Value = SlotViewModel.amountOfJackpot },
			new WinDataModel { Category = "Small Wins", Value = SlotViewModel.amountOfSmallWins },
			new WinDataModel { Category = "Losses", Value = SlotViewModel.amountOfLosses }
		};

		[ObservableProperty]
		public ObservableCollection<Record> records = new ObservableCollection<Record>();

        [ObservableProperty] private double totalWinnings;

		public void Reinitialize()
		{
			Records = new ObservableCollection<Record>(_gameService.records);
			Windata.Clear();
            Windata.Add(new WinDataModel { Category = "Jackpot Wins", Value = SlotViewModel.amountOfJackpot });
            Windata.Add(new WinDataModel { Category = "Small Wins", Value = SlotViewModel.amountOfSmallWins });
            Windata.Add(new WinDataModel { Category = "Losses", Value = SlotViewModel.amountOfLosses });
        }
        public StatisticsViewModel(GameService gameService)
        {
            _gameService = gameService;
        }
    }
}
