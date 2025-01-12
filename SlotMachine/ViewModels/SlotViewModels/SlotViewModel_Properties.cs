using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SlotMachine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels.SlotViewModels
{
    public partial class SlotViewModel : ObservableObject
    {

        private readonly GameService _gameService;
        private readonly List<string> Symbols = new List<string> { "🍎", "🍒", "🍇", "🍊", "🔔", "💎" };
        private Random random = new Random();
        public bool freeSpin = true;
        public double _multiplier = 1.2;
        public double jackpot = 100;
        private DateTime _lastRefillTime;
        private const int RefillIntervalSeconds = 60;
        private bool isSpinning;
        public ObservableCollection<string> ThemeCollection { get; set; } = new ObservableCollection<string>
        {
            "Standard",
            "Desert"
        };

        [ObservableProperty]
        public ObservableCollection<string> reel1Stack = new ObservableCollection<string>() { "🍎", "🍒", "🍇" };

        [ObservableProperty]
        public ObservableCollection<string> reel2Stack = new ObservableCollection<string>() { "🍎", "🍒", "🍇" };

        [ObservableProperty]
        public ObservableCollection<string> reel3Stack = new ObservableCollection<string>() { "🍎", "🍒", "🍇" };

        [ObservableProperty]
        private View? animationContent;

        [ObservableProperty]
        private string amount = "10";

        [ObservableProperty]
        private int credits = 100;

        [ObservableProperty]
        private string spinBtnText = "Free Spin";

        [ObservableProperty]
        private string timeLeftForRefill = $"Next refill in {60 - DateTime.Now.Second} seconds.";

        [ObservableProperty]
        private bool isChecked = false;
        [ObservableProperty]
        private bool canSpin = true;
        [ObservableProperty]
        private bool isAnimationEnabled = false;
        [ObservableProperty]
        private bool isAnimationVisible = false;

        private bool _isUpdatingAmount = false;
        [ObservableProperty]
        private string currentTheme = "Standard";

        public static int amountOfSmallWins { get; set; } = 0;
        public static int amountOfJackpot { get; set; } = 0;
        public static int amountOfLosses { get; set; } = 0;


        public CollectionView Reel1View { get; set; } = new CollectionView();
        public CollectionView Reel2View { get; set; } = new CollectionView();
        public CollectionView Reel3View { get; set; } = new CollectionView();

        partial void OnAmountChanged(string value)
        {
            if (_isUpdatingAmount)
                return;

            _isUpdatingAmount = true;
            try
            {
                Amount = ValidateAmount(value);
                if (!freeSpin)
                {
                    SpinBtnText = Amount + " Credits";
                    OnPropertyChanged(nameof(SpinBtnText));
                }
            }
            finally
            {
                _isUpdatingAmount = false;
            }

        }

        private string ValidateAmount(string value)
        {
            if (!int.TryParse(value, out int creditsToSpin) || creditsToSpin < 1)
                return "1";
            return Math.Min(creditsToSpin, Credits).ToString();
        }
    }
}
