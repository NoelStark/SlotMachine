using SlotMachine.Services;
using SlotMachine.ViewModels;
using SlotMachine.ViewModels.SlotViewModels;

namespace SlotMachine.Views;

public partial class StatisticsView : ContentPage
{
	StatisticsViewModel viewModel;
    public StatisticsView(GameService gameService)
	{
		

		InitializeComponent();
		viewModel = new StatisticsViewModel(gameService);
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.Reinitialize();
    }
}