using System.Collections.Generic;

namespace Chronometer.Contracts
{
    public interface IChronometer
    {
        public string GetTime { get; }
        public List<string> Laps { get; }

        void Start();
        void Stop();

        string Lap();

        void Reset();
    }
}
