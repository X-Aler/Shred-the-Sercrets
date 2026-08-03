using System;

namespace DefaultNamespace
{
    public class DocumentTimer
    {
        public int TimeLeft { get; private set; }

        public event Action<int> TimeChanged;

        public void SetTime(int time)
        {
            TimeLeft = time;
            TimeChanged?.Invoke(TimeLeft);
        }
    }
}