using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace SlotMachine.ViewModels.SlotViewModels
{
    public partial class SlotViewModel : ObservableObject
    {
        [RelayCommand(CanExecute = nameof(CanSpin))]
        public async Task Spin()
        {
            CostToSpin();
            await _gameService.PlaySound("slotmachinesound.wav");
            Vibration.Vibrate(TimeSpan.FromSeconds(8));
            var reel1Task = SpinReel(Reel1Stack, 0, Reel1View);
            var reel2Task = SpinReel(Reel2Stack, 200, Reel2View);
            var reel3Task = SpinReel(Reel3Stack, 400, Reel3View);

            await Task.WhenAll(reel1Task, reel2Task, reel3Task);
            Vibration.Cancel();
            _gameService.SpinReels();
            _gameService.StopSound();
            await CheckWinnings();
            if ((Credits - int.Parse(Amount)) < 0)
            {
                CanSpin = false;
                SpinCommand.NotifyCanExecuteChanged();
            }

            if (IsChecked && CanSpin)
                await Spin();
        }

        [RelayCommand]
        void HalfAmount()
        {
            if(int.TryParse(Amount, out var currentAmount))
            {
                int newAmount = Math.Max(1, currentAmount / 2);
                Amount = newAmount.ToString();
            }
        }

        [RelayCommand]
        void DoubleAmount()
        {
            if (int.TryParse(Amount, out var currentAmount))
            {
                int newAmount = Math.Min(Credits, currentAmount * 2);
                Amount = newAmount.ToString();
            }
        }
        [RelayCommand]
        void MaxAmount()
        {
            Amount = Credits.ToString();
        }
    }
}
