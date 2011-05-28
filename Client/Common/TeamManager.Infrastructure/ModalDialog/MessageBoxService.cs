using System.Windows;

namespace TeamManager.Infrastructure.ModalDialog
{
    public class MessageBoxService : IMessageBoxService
    {
        #region Implementation of IMessageBoxService

        public GenericMessageBoxResult Show(string message, string caption, GenericMessageBoxButton buttons)
        {
            var slButtons = buttons == GenericMessageBoxButton.Ok
                                ? MessageBoxButton.OK
                                : MessageBoxButton.OKCancel;

            var result = MessageBox.Show(message, caption, slButtons);
            return result == MessageBoxResult.OK ? GenericMessageBoxResult.Ok : GenericMessageBoxResult.Cancel;
        }

        public void Show(string message, string caption)
        {
            Show(message, caption, GenericMessageBoxButton.OkCancel);
        }

        #endregion
    }
}