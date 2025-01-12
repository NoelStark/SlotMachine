using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Models
{
    public partial class WinDataModel : ObservableObject
    {
        [ObservableProperty]
        private string category;

        [ObservableProperty]
        private double value;
    }
}
