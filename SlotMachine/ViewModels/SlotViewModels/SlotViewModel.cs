
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
        private readonly IVibrationService _vibrationService;
        public SlotViewModel(GameService gameService, IVibrationService service)
        {
            _gameService = gameService;
            _vibrationService = service;
            RefillTimer();
        }

        /// <summary>
        /// First spin is free but after that it changes the current credits
        /// to be reduced based on the input amount
        /// </summary>
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

            //Groups results to see how many matching symbols there are
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
                amountOfLosses++;
                UpdateMultiplierAndJackpot();
            }

			OnPropertyChanged(nameof(Credits));
		    freeSpin = false;

        }

        /// <summary>
        /// If 3 symbols match, the win amount is calculated, sound is being played
        /// and a record of the jackpot is saved in the game service
        /// </summary>
        /// <returns></returns>
        private async Task HandleJackpot()
		{
            await _gameService.PlaySound("confetti.wav");
            PlayWinningAnimation();
            amountOfJackpot++;
            Credits += Convert.ToInt16(jackpot);
            _gameService.CreateRecord("Jackpot", jackpot);
            jackpot = 100;
        }

        /// <summary>
        /// If 2 symbols match, the win amount is calculated, sound is being played
        /// and a record of the win is saved in the game service
        /// </summary>
        /// <returns></returns>
		private async Task HandleWin()
		{
            await _gameService.PlaySound("confetti.wav");

            PlayWinningAnimation();
            double winAmount = (Convert.ToInt32(Amount) * _multiplier);
            if (freeSpin && winAmount > 50)
                Credits += 50;
            else
            {
                amountOfSmallWins++;
                Credits += Convert.ToInt32(winAmount);
            }
            _gameService.CreateRecord("Small Win", winAmount);
        }

        /// <summary>
        /// Every time the user loses, the jackpot and multiplier gets increased
        /// </summary>
        private void UpdateMultiplierAndJackpot()
        {
            if (_multiplier < 10)
                _multiplier += 0.1;
            jackpot += 100;
        }

        //Plays the Lottie Confetti animation
        public void PlayWinningAnimation()
        {
            IsAnimationEnabled = true;
            IsAnimationVisible = true;
            OnPropertyChanged(nameof(IsAnimationEnabled));
            OnPropertyChanged(nameof(IsAnimationVisible));
        }

        /// <summary>
        /// It takes the stack, spins it and randomize what symbol that gets rolled
        /// </summary>
        /// <param name="reelStack"></param>
        /// <param name="delay"></param>
        /// <param name="reelView"></param>
        /// <returns></returns>
        private async Task SpinReel(ObservableCollection<string> reelStack, int delay, CollectionView reelView)
		{
			if (delay > 0)
				await Task.Delay(delay);

			int totalFrames = 30;
			int interval = 50;
            //This loop creates the 'animation' of them spinning
			for (int i = 0; i < totalFrames; i++)
			{
				
				var targetIndex = (i + 1) % reelStack.Count;
				string nextSymbol;
                //Visually change them until the for-loop is done
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
        /// <summary>
        /// This method creates a subscriber of which it sends a message to
        /// every 1 second. After 60 seconds, amount of credits gets increased
        /// </summary>
		private void RefillTimer()
		{
           
            _lastRefillTime = DateTime.Now;

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    var elapsedSeconds = (DateTime.Now - _lastRefillTime).TotalSeconds;
                    //If the 60 seconds has passed
                    if (elapsedSeconds >= RefillIntervalSeconds)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Credits += 20;
                            _lastRefillTime = DateTime.Now;
                            if(Amount == "0")
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

       
        private void SetTheme(string theme)
        {
            var mergedDict = Application.Current?.Resources.MergedDictionaries;
            mergedDict.Clear();
            switch (theme)
            {
                case "Standard":
                    mergedDict.Add(new SlotMachine.Resources.Themes.Standard());
                    break;
                case "Desert":
                    mergedDict.Add(new SlotMachine.Resources.Themes.Desert());

                    break;
            }
        }
    }
}
