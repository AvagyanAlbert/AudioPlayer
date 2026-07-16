namespace AudioPlayer.Services
{
    public interface IMusicPlayer
    {
        public void Open(string? path);
        public void Play();
        public void Pause();
        public void Stop();
        public event Action<double>? ProgressChanged;
        public event Action<string, string>? TimeChanged;
        public event Action? MediaOpened;
    }
}
