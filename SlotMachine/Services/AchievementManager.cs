using SlotMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Services
{
    public class AchievementManager
    {
        private readonly ObservableCollection<Achievement> _achievements = new ObservableCollection<Achievement>
        {
             new Achievement("JackpotHunter", "Jackpot Hunter", "Win the Jackpot 1 time", 1),
                new Achievement("SpinBeginner", "And so it begins", "Spin the reel 1 time", 1),
                new Achievement("SpinIntermediate", "The addiction continues", "Spin the reel 10 times", 10),
                new Achievement("SpinMaster", "Really?", "Spin the reel 100 times", 100)
        };

        public void StartAchievementsProgress(string achievementId)
        {
            var achievement = _achievements.FirstOrDefault(a => a.Id == achievementId);
            achievement?.Start();
        }

        public void UpdateAchievementsProgress(string achievementId, int increment)
        {
            var achievement = _achievements.FirstOrDefault(a => a.Id == achievementId);
            achievement?.Update(increment);
        }

        public ObservableCollection<Achievement> GetAchievements() => _achievements;
    }
}
