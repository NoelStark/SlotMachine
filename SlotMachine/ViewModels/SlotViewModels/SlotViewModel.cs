
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp.Extended.UI.Controls;
using SlotMachine.Models;
using SlotMachine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels.SlotViewModels
{
	public partial class SlotViewModel
	{
        public SlotViewModel(GameService gameService)
        {
            _gameService = gameService;
            RefillTimer();
        }

		private void CostToSpin()
		{
            if (!freeSpin)
                Credits -= Convert.ToInt32(Amount);
            SpinBtnText = Amount + " Credits";
            OnPropertyChanged(nameof(SpinBtnText));
            OnPropertyChanged(nameof(Credits));
        }
	
		public async Task CheckWinnings()
		{
			string[] results = new[] { Reel1Stack[1], Reel2Stack[1], Reel3Stack[1] };

            var maxMatchCount = results.GroupBy(x => x).Max(x => x.Count());

			if(maxMatchCount == 3)
			{
				await HandleJackpot();
            }
            else if (maxMatchCount == 2)
			{
                await HandleWin();
            }
            else
			{
                UpdateMultiplierAndJackpot();
            }

			OnPropertyChanged(nameof(Credits));
		    freeSpin = false;

        }

		private async Task HandleJackpot()
		{
            await _gameService.PlaySound("confetti.wav");
             PlayWinningAnimation();
            Credits += Convert.ToInt16(jackpot);
            _gameService.CreateRecord("Jackpot", jackpot);
            jackpot = 100;
        }

		private async Task HandleWin()
		{
            await _gameService.PlaySound("confetti.wav");

            PlayWinningAnimation();
            double winAmount = (Convert.ToInt32(Amount) * _multiplier);
            if (freeSpin && winAmount > 50)
                Credits += 50;
            else
            {
                Credits += Convert.ToInt32(winAmount);
            }
            _gameService.CreateRecord("Small Win", winAmount);
        }

        private void UpdateMultiplierAndJackpot()
        {
            if (_multiplier < 10)
                _multiplier += 0.1;
            jackpot += 100;
        }

        public void PlayWinningAnimation()
        {
            IsAnimationEnabled = true;
            IsAnimationVisible = true;
            OnPropertyChanged(nameof(IsAnimationEnabled));
            OnPropertyChanged(nameof(IsAnimationVisible));
        }


        private async Task SpinReel(ObservableCollection<string> reelStack, int delay, CollectionView reelView)
		{
			if (delay > 0)
				await Task.Delay(delay);

			int totalFrames = 30;
			int interval = 50;

			for (int i = 0; i < totalFrames; i++)
			{
				
				var targetIndex = (i + 1) % reelStack.Count;
				string nextSymbol;
				do
				{
					nextSymbol = Symbols[random.Next(Symbols.Count)];
				}
				while (reelStack[targetIndex] == nextSymbol);
				reelStack[targetIndex] = nextSymbol;		
				reelView.ScrollTo(targetIndex, position: ScrollToPosition.MakeVisible, animate: true);

				interval = Math.Min(150, interval + 5);
				await Task.Delay(interval);
			}
		}

		private void RefillTimer()
		{
           
            _lastRefillTime = DateTime.Now;

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    var elapsedSeconds = (DateTime.Now - _lastRefillTime).TotalSeconds;

                    if (elapsedSeconds >= RefillIntervalSeconds)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Credits += 20;
                            _lastRefillTime = DateTime.Now;
                            Amount = Credits.ToString();
                            CanSpin = true;
                            SpinCommand.NotifyCanExecuteChanged();
                        });
                    }
                    UpdateTimer(elapsedSeconds);
                });
        }
        private void UpdateTimer(double elapsedSeconds)
        {
            TimeLeftForRefill = $"Next refill in {RefillIntervalSeconds - (int)elapsedSeconds} seconds.";
        }

        public void Shake_Sensor()
        {
            Accelerometer.Default.ShakeDetected += OnShakeDetected;
            Accelerometer.Default.Start(SensorSpeed.Game);
        }
        public void StopShakeDetect()
        {
            Accelerometer.Default.ShakeDetected -= OnShakeDetected;
            Accelerometer.Default.Stop();
        }

        private void OnShakeDetected(object sender, EventArgs args)
        {
            _= Spin();
        }

    }
}
