using AudioPlayer.Commands;
using AudioPlayer.Models;
using AudioPlayer.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AudioPlayer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IMusicPlayer player;
        public ICommand PlayPauseCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        private int currentIndex = 0;
        private bool isPlaying;
        public List<Song> Songs { get; set; }

        private Song? currentSong;
        public Song? CurrentSong
        {
            get => currentSong;
            set
            {
                currentSong = value;
                OnPropertyChanged(nameof(CurrentSong));
            }
        }

        private double songProgress;
        public double SongProgress
        {
            get => songProgress;
            set
            {
                songProgress = value;
                OnPropertyChanged(nameof(SongProgress));
            }
        }

        private string currentTime = "00:00";
        public string CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        private string totalTime = "00:00";
        public string TotalTime
        {
            get => totalTime;
            set
            {
                totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }
        public MainViewModel(IMusicPlayer musicPlayer)
        {
            player = musicPlayer ?? throw new ArgumentNullException(nameof(musicPlayer));

            PlayPauseCommand = new Relay(PlayPause);
            NextCommand = new Relay(NextSong);
            PreviousCommand = new Relay(PreviousSong);

            Songs = new List<Song>
            {
                new ()
                {
                    Title = "Better Days",
                    Artist = "LAKEY INSPIRED",
                    AudioPath = "Assets/BetterDays.mp3",
                    ImagePath = "Assets/BetterDays.jpg"
                },
                new ()
                {
                    Title = "A Walk",
                    Artist = "Tycho",
                    AudioPath ="Assets/A_Walk.mp3",
                    ImagePath = "Assets/A_Walk.jpg"
                }
            };

            CurrentSong = Songs[currentIndex];
            player.Open(CurrentSong!.AudioPath);

            player.ProgressChanged += value =>
                {
                    SongProgress = value;
                };

            player.TimeChanged += (current, total) =>
                {
                    CurrentTime = current;
                    TotalTime = total;
                };
        }

        private void PlayPause()
        {
            if (isPlaying)
            {
                player.Pause();
                isPlaying = false;
            }
            else
            {
                player.Open(CurrentSong!.AudioPath);
                player.Play();
                isPlaying = true;
            }

            PlayButtonText = isPlaying ? "⏸" : "▶";
            HeaderText = isPlaying ? "Now Playing" : "Paused";
        }

        private void NextSong()
        {
            currentIndex++;

            if (currentIndex >= Songs.Count)
                currentIndex = 0;

            SwitchSong();
        }

        private void PreviousSong()
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = Songs.Count - 1;

            SwitchSong();
        }

        private void SwitchSong()
        {
            CurrentSong = Songs[currentIndex];
            player.Open(CurrentSong!.AudioPath);
            if (isPlaying)
                player.Play();
        }

        private string playButtonText = "▶";
        public string PlayButtonText
        {
            get => playButtonText;
            set { playButtonText = value; OnPropertyChanged(nameof(PlayButtonText)); }
        }
        private string nextButtonText = "⏭ ";
        public string NextButtonText
        {
            get => nextButtonText;
            set { nextButtonText = value; OnPropertyChanged(nameof(NextButtonText)); }
        }
        private string previousButtonText = "⏮ ";
        public string PreviousButtonText
        {
            get => previousButtonText;
            set { previousButtonText = value; OnPropertyChanged(nameof(PreviousButtonText)); }
        }
        private string headerText = "";
        public string HeaderText
        {
            get => headerText;
            set { headerText = value; OnPropertyChanged(nameof(HeaderText)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}