using Microsoft.Practices.Prism.Commands;

namespace TeamManager.Infrastructure.Commands
{
    public static class CommandsUtils
    {
        public static void UpdateCanExecute(this DelegateCommand command)
        {
            command.RaiseCanExecuteChanged();
        }
    }
}