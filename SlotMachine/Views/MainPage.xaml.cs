using SlotMachine.ViewModels;
using SlotMachine.ViewModels.SlotViewModels;
namespace SlotMachine.Views
{
	public partial class MainPage : ContentPage
    {
        private SlotViewModel _viewModel;
		public MainPage(SlotViewModel viewModel)
		{
			InitializeComponent();
			_viewModel = viewModel;

            _viewModel.Reel1View = Reel1View;
            _viewModel.Reel2View = Reel2View;
            _viewModel.Reel3View = Reel3View;
			BindingContext = _viewModel;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.StopShakeDetect();
        }

        protected override void OnAppearing()
        { 
            base.OnAppearing();
            _viewModel.Shake_Sensor();
        }
    }

}
