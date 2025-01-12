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
        /// <summary>
        /// This method is responsible for everything that happens after a click.
        /// Reels gets spun, vibrations starts and a sound is being played
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanSpin))]
        public async Task Spin()
        {
            if (isSpinning) return;
            isSpinning = true;
            CostToSpin();
            await _gameService.PlaySound("slotmachinesound.wav");
            //If the current device has vibration supported, start the vibration
            if (_vibrationService.IsSupported)
            {
                _vibrationService.Vibrate(TimeSpan.FromSeconds(8));
            }
            //Spin each reel with a slight delay between them all
            var reel1Task = SpinReel(Reel1Stack, 0, Reel1View);
            var reel2Task = SpinReel(Reel2Stack, 200, Reel2View);
            var reel3Task = SpinReel(Reel3Stack, 400, Reel3View);

            await Task.WhenAll(reel1Task, reel2Task, reel3Task);

            if (_vibrationService.IsSupported)
                _vibrationService.Cancel();
            
            _gameService.SpinReels();
            _gameService.StopSound();
            await CheckWinnings();
            //If the user run outs of credits, one can no longer spin
            if ((Credits - int.Parse(Amount)) < 0)
            {
                CanSpin = false;
                SpinCommand.NotifyCanExecuteChanged();
            }

            isSpinning = false;
            if (IsChecked && CanSpin)
                await Spin();
        }

        [RelayCommand]
        void HalfAmount()
        {
            //When the 1/2 button is clicked, the cost to spin is halved
            if(int.TryParse(Amount, out var currentAmount))
            {
                int newAmount = Math.Max(1, currentAmount / 2);
                Amount = newAmount.ToString();
            }
        }

        [RelayCommand]
        void DoubleAmount()
        {
            //When the 2x button is clicked, the cost to spin is doubled
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

        [RelayCommand]
        void SwitchTheme()
        {
            if (CurrentTheme == "Standard") CurrentTheme = "Desert";
            else if (CurrentTheme == "Desert") CurrentTheme = "Standard";
            SetTheme(CurrentTheme);
        }
    }
}
