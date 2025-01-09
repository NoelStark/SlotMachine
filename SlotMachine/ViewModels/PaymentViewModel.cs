using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SlotMachine.Models;
using SlotMachine.ViewModels.SlotViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels
{
    [QueryProperty(nameof(SerializedItem), "item")]
    public partial class PaymentViewModel : ObservableObject
    {
        private readonly SlotViewModel _slotViewModel;
        [ObservableProperty]
        private string serializedItem;

        public PaymentViewModel(SlotViewModel slotViewModel)
        {
            _slotViewModel = slotViewModel;
        }
        [RelayCommand]
        void Pay()
        {

            ShopItem? shopItem = JsonConvert.DeserializeObject<ShopItem>(SerializedItem);
            if(shopItem != null)
            {
                _slotViewModel.Credits += (int)double.Parse(shopItem.Credits);
            }  
        }
    }
}
