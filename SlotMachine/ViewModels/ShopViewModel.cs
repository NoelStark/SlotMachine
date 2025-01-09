using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SlotMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels
{
    public partial class ShopViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<ShopItem> shopItems = new ObservableCollection<ShopItem>()        
        {
                new ShopItem { ImageSource = "coin1.png", Credits = "100.00", Price = "5" },
                new ShopItem { ImageSource = "coin2.png", Credits = "300.00", Price = "10" },
                new ShopItem { ImageSource = "coin3.png", Credits = "450.00", Price = "15" },
                new ShopItem { ImageSource = "coin4.png", Credits = "800.00", Price = "20" },
                new ShopItem { ImageSource = "coin5.png", Credits = "1200.00", Price = "30" }
        };

        [RelayCommand]
        async Task BuyItem(ShopItem item)
        {
            string serializedItem = JsonConvert.SerializeObject(item);

            await Shell.Current.GoToAsync($"///PaymentView?item={Uri.EscapeDataString(serializedItem)}", true);
        }
    }
}
