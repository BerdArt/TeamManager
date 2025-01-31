namespace TeamManager.Infrastructure.ModalDialog
{
    public interface IMessageBoxService
    {
        GenericMessageBoxResult Show(string message, string caption, GenericMessageBoxButton buttons);
        void Show(string message, string caption);
    }

    public enum GenericMessageBoxResult
    {
        Ok,
        Cancel
    }

    public enum GenericMessageBoxButton
    {
        Ok,
        OkCancel
    }
}