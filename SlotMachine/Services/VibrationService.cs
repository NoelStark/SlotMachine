using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Services
{
    public interface IVibrationService
    {
        void Vibrate(TimeSpan duration);
        void Cancel();
        bool IsSupported { get; }
    }
    public class VibrationService : IVibrationService
    {
        public void Vibrate(TimeSpan duration)
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(duration);
            }
        }

        public void Cancel()
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Cancel();
            }
        }

        public bool IsSupported => Vibration.Default.IsSupported;
    }
}
