using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.ViewModels
{
    public partial class ThemeViewModel : ObservableObject
    {
        [RelayCommand]
        public void SwitchToStandard()
        {
            SetTheme("Standard");
        }

        [RelayCommand]
        public void SwitchToDesert()
        {
            SetTheme("Desert");
        }

        private void SetTheme(string theme)
        {
            var mergedDict = Application.Current?.Resources.MergedDictionaries;
            mergedDict.Clear();
            switch (theme)
            {
                case "Standard":
                    mergedDict.Add(new SlotMachine.Resources.Themes.Standard());
                    break;
                case "Desert":
                    mergedDict.Add(new SlotMachine.Resources.Themes.Desert());

                    break;
            }
        }
    }
}
