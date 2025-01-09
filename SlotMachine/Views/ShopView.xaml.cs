using SlotMachine.ViewModels;

namespace SlotMachine.Views;

public partial class ShopView : ContentPage
{
	public ShopView(ShopViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}