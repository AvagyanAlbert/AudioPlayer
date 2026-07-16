using System.Windows.Input;

namespace AudioPlayer.Commands
{
    public class Relay(Action execute) : ICommand
    {
        private readonly Action execute = execute ?? throw new ArgumentNullException(nameof(execute));
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => execute();
    }
}
