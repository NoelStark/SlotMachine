using Plugin.Maui.Audio;
using SlotMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Services
{
    public class GameService
    {
        private readonly AchievementManager _achievementManager;
        private AudioManager _audioManager = new AudioManager();
        private IAudioPlayer? _currentSong;
        public List<Record> records { get; private set; } = new List<Record>();
        public GameService(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }

        public void CreateRecord(string category, double value)
        {
            records.Add(new Record { Category = category, Value = value });
        }

        public void SpinReels()
        {
            _achievementManager.UpdateAchievementsProgress("SpinBeginner",1);
            _achievementManager.UpdateAchievementsProgress("SpinIntermediate",1);
            _achievementManager.UpdateAchievementsProgress("SpinMaster",1);
        }

        public void HitJackpot()
        {
            _achievementManager.UpdateAchievementsProgress("JackpotHunter", 1);
        }

        public async Task PlaySound(string sound)
        {
            _currentSong = _audioManager.CreatePlayer(sound);

            await Task.Run(()=> _currentSong.Play());
        }

        public void StopSound()
        {
            if (_currentSong != null)
                _currentSong.Dispose();
        }
    }
}
