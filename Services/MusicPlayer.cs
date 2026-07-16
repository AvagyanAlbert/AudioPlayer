using System.IO;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioPlayer.Services
{
    public class MusicPlayer : IMusicPlayer
    {
        private readonly MediaPlayer player = new();
        private readonly DispatcherTimer timer;
        public bool AutoPlayRequested { get; set; }
        public event Action<double>? ProgressChanged;
        public event Action<string, string>? TimeChanged;
        public event Action? MediaOpened;

        public MusicPlayer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };

            timer.Tick += Timer_Tick;
            player.MediaOpened += (s, e) => MediaOpened?.Invoke();
        }

        public void Open(string? path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            timer.Stop();
            string fullPath = Path.GetFullPath(path);
            player.Open(new Uri(fullPath));
            ProgressChanged?.Invoke(0);
            TimeChanged?.Invoke("00:00", "00:00");
        }

        public void Play()
        {
            player.Play();
            timer.Start();
        }

        public void Pause()
        {
            player.Pause();
            timer.Stop();
        }
        public void Stop()
        {
            player.Stop();
            timer.Stop();

            ProgressChanged?.Invoke(0);
            TimeChanged?.Invoke("00:00", "00:00");
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!player.NaturalDuration.HasTimeSpan)
                return;

            double total = player.NaturalDuration.TimeSpan.TotalSeconds;
            if (total <= 0)
                return;

            double current = player.Position.TotalSeconds;
            double progress = (current / total) * 100;

            ProgressChanged?.Invoke(progress);

            TimeChanged?.Invoke(
                FormatTime(current),
                FormatTime(total)
            );
        }

        private static string FormatTime(double sec)
        {
            return TimeSpan.FromSeconds(sec).ToString(@"mm\:ss");
        }
    }
}