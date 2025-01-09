
using SlotMachine.ViewModels;

namespace SlotMachine.Views;

public partial class AchievementView : ContentPage
{
	AchievementsViewModel _viewModel;
	public AchievementView(AchievementsViewModel viewModel)
	{
		_viewModel = viewModel;
		InitializeComponent();
		BindingContext = _viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.Reinitialize();
    }
}