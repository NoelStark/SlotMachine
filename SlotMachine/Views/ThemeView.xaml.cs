using SlotMachine.ViewModels;

namespace SlotMachine.Views;

public partial class ThemeView : ContentPage
{
	public ThemeView(ThemeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}