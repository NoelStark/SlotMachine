using CommunityToolkit.Mvvm.ComponentModel;
using SlotMachine.Models;
using SlotMachine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SlotMachine.ViewModels
{
    public partial class AchievementsViewModel : ObservableObject
    {
        private readonly AchievementManager _achievementManager;

        public void Reinitialize()
        {
            Achievements.Clear();
            foreach (var achievement in _achievementManager.GetAchievements())
            {
                Achievements.Add(achievement); // Refresh the achievements
            }
        }

        public AchievementsViewModel(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }
        [ObservableProperty]
        public ObservableCollection<Achievement> achievements = new ObservableCollection<Achievement> ();
    }
}
