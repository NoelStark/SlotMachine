using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Stateless;
namespace SlotMachine.Models
{
    public partial class Achievement : ObservableObject
    {
        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private double goal;

        [ObservableProperty]
        private double progress;
        public string ProgressText => $"{Progress}/{Goal}";
        public double ProgressPrecentage => Goal == 0 ? 0 : Progress / Goal;
        public AchievementState CurrentState => _stateMachine.State;

        private readonly StateMachine<AchievementState, AchievementTrigger> _stateMachine;

        public Achievement(string id, string name, string description, int goal)
        {
            Id = id;
            Name = name;
            Description = description;
            Goal = goal;
            Progress = 0;
            _stateMachine = new StateMachine<AchievementState, AchievementTrigger>(AchievementState.InProgress);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(AchievementState.InProgress)
                .PermitReentry(AchievementTrigger.UpdateProgress)
                .PermitIf(AchievementTrigger.Complete, AchievementState.Unlocked, () => Progress >= Goal);

            _stateMachine.Configure(AchievementState.Unlocked)
                .OnEntry(() => 
                {
                    OnPropertyChanged(nameof(CurrentState));
                });
        }

        public void Start()
        {
            _stateMachine.Fire(AchievementTrigger.StartProgress);
        }
        public void Update(int increment)
        {
            if (CurrentState != AchievementState.InProgress) return;

            Progress += increment;
            OnPropertyChanged(nameof(ProgressPrecentage));
            OnPropertyChanged(nameof(ProgressText));

            if (Progress >= Goal)
            {
                _stateMachine.Fire(AchievementTrigger.Complete);
            }
            else
            {
                _stateMachine.Fire(AchievementTrigger.UpdateProgress);
            }
        }
    }
}
