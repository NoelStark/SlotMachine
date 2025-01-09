using Moq;
using SlotMachine.Services;
using SlotMachine.ViewModels.SlotViewModels;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace UnitTest
{
    public class UnitTest1
    {
        private readonly Mock<AchievementManager> _achievementMock;
        private readonly Mock<GameService> _gameServiceMock;
        private readonly SlotViewModel _slotViewModel;
        public UnitTest1()
        {
            _achievementMock = new Mock<AchievementManager>();
            _gameServiceMock = new Mock<GameService>(_achievementMock.Object);
            _slotViewModel = new SlotViewModel(_gameServiceMock.Object)
            {
                Reel1Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" },
                Reel2Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" },
                Reel3Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" },
                Credits = 100,
                Amount = "10"
            };
        }
        [Fact]
        public async void Spin_ShouldReduceCredits()
        {
            _slotViewModel.IsChecked = false;
            var initialCredits = _slotViewModel.Credits;

            await _slotViewModel.Spin();
            await _slotViewModel.Spin();
            
            bool changedValue = !initialCredits.Equals(_slotViewModel.Credits);

            Assert.True(changedValue, "Value should have changed after spin");
        }
        [Fact]
        public async void Spin_ShouldSpinReels()
        {
            var initalReel1 = new List<string>(_slotViewModel.Reel1Stack);
            var initalReel2 = new List<string>(_slotViewModel.Reel2Stack);
            var initalReel3 = new List<string>(_slotViewModel.Reel3Stack);

            await _slotViewModel.Spin();
            bool reel1Changed = !initalReel1.SequenceEqual(_slotViewModel.Reel1Stack);
            bool reel2Changed = !initalReel2.SequenceEqual(_slotViewModel.Reel2Stack);
            bool reel3Changed = !initalReel3.SequenceEqual(_slotViewModel.Reel3Stack);

            Assert.True(reel1Changed || reel2Changed || reel3Changed, "Atleast one reel should have changed after spinning");
        }

        [Fact]
        public async void Spin_IncreaseCreditsOnJackpot()
        {
            var initialCredits = _slotViewModel.Credits;
            _slotViewModel.Reel1Stack = new ObservableCollection<string> { "🍎", "🍎", "🍎" };
            _slotViewModel.Reel2Stack = new ObservableCollection<string> { "🍎", "🍎", "🍎" };
            _slotViewModel.Reel3Stack = new ObservableCollection<string> { "🍎", "🍎", "🍎" };

            _slotViewModel.Amount = "10";
            var betAmount = int.Parse(_slotViewModel.Amount);
            await _slotViewModel.CheckWinnings();

            var win = (int)initialCredits + _slotViewModel.jackpot;

            Assert.Equal(win, _slotViewModel.Credits);
            Assert.Equal(100, _slotViewModel.jackpot);
        }

        [Fact]
        public async void Spin_IncreaseCreditsOnSmallWin()
        {
            var initialCredits = _slotViewModel.Credits;
            _slotViewModel.Reel1Stack = new ObservableCollection<string> { "🍎", "🍎", "🍇" };
            _slotViewModel.Reel2Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" };
            _slotViewModel.Reel3Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" };

            _slotViewModel.Amount = "10";
            var betAmount = int.Parse(_slotViewModel.Amount);
            await _slotViewModel.CheckWinnings();

            var win = (int)betAmount * _slotViewModel._multiplier;

            Assert.Equal(initialCredits + win, _slotViewModel.Credits);
            Assert.True(_slotViewModel._multiplier >= 1.2, "Multiplier should be 1.2");
        }
        [Fact]
        public async void Spin_IncreaseMultiplierAndJackpot()
        {
            var multiplier = _slotViewModel._multiplier;
            var jackpot = _slotViewModel.jackpot;
            _slotViewModel.Reel1Stack = new ObservableCollection<string> { "🍎", "🍎", "🍇" };
            _slotViewModel.Reel2Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" };
            _slotViewModel.Reel3Stack = new ObservableCollection<string> { "🍎", "🍇", "🍇" };
            await _slotViewModel.CheckWinnings();
            Assert.Equal(multiplier + 0.1, _slotViewModel._multiplier);
            Assert.Equal(jackpot + 100, _slotViewModel.jackpot);
        }
        [Fact]
        public async void Spin_LimitCreditsOnFreeWin()
        {
            var multiplier = _slotViewModel._multiplier;
            var initialCredits = _slotViewModel.Credits;
            _slotViewModel.freeSpin = true;
            _slotViewModel.Amount = "100";
            _slotViewModel.Reel1Stack = new ObservableCollection<string> { "🍎", "🍎", "🍇" };
            _slotViewModel.Reel2Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" };
            _slotViewModel.Reel3Stack = new ObservableCollection<string> { "🍎", "🍒", "🍇" };
            await _slotViewModel.CheckWinnings();
            var win = (Convert.ToInt32(_slotViewModel.Amount) * _slotViewModel._multiplier);
            var expectedAmount = win + initialCredits;

            Assert.Equal(150, _slotViewModel.Credits);


        }

    }
}