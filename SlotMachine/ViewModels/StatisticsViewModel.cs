using CommunityToolkit.Mvvm.ComponentModel;
using SlotMachine.Models;
using SlotMachine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels
{
	public partial class StatisticsViewModel : ObservableObject
	{
		private GameService _gameService;
		
		public ObservableCollection<WinDataModel> Windata { get; set; } = new ObservableCollection<WinDataModel>
		{
			new WinDataModel { Category = "Jackpot Wins", Value = 5 },
			new WinDataModel { Category = "Small Wins", Value = 20 },
			new WinDataModel { Category = "Losses", Value = 75 }
		};

		[ObservableProperty]
		public ObservableCollection<Record> records = new ObservableCollection<Record>();

		public void Reinitialize()
		{
			Records = new ObservableCollection<Record>(_gameService.records);
		}
        public StatisticsViewModel(GameService gameService)
        {
            _gameService = gameService;
        }
    }
}
