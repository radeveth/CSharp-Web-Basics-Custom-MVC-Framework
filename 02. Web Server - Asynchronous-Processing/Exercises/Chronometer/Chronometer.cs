using Chronometer.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chronometer
{
    public class Chronometer : IChronometer
    {
        private Stopwatch watch;

        public Chronometer()
        {
            this.watch = new Stopwatch();
            this.Laps = new List<string>();
        }

        public string GetTime { get => GetCurrentTime(); }


        public List<string> Laps { get; }

        public string Lap()
        {
            var currentLapToString = this.Format(this.watch.Elapsed);
            this.Laps.Add(currentLapToString);

            return currentLapToString;
        }

        public void Reset()
        {
            this.watch.Reset();
            this.Laps.Clear();
            this.Start();
        }

        public void Start()
        {
            this.watch.Start();
        }

        public void Stop()
        {
            this.watch.Stop();
        }

        public string GetLaps()
        {
            if (this.Laps.Count == 0)
            {
                return "No existing laps!";
            }

            var laps = new StringBuilder();
            foreach (var lap in this.Laps)
            {
                laps.AppendLine(lap);
            }

            return laps.ToString().TrimEnd();
        }
        private string GetCurrentTime()
        {
            var currentTime = this.watch.Elapsed;
            return this.Format(currentTime);
        }

        private string Format(TimeSpan currentTime)
        {
            return $"{currentTime.Minutes}:{currentTime.Seconds}:{currentTime.Milliseconds}";
        }
    }
}
