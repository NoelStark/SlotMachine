using SlotMachine.ViewModels;

namespace SlotMachine.Views;

public partial class PaymentView : ContentPage
{
	public PaymentView(PaymentViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}