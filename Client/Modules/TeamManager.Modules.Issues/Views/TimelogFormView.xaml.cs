using System.Windows;
using TeamManager.Infrastructure.ModalDialog;

namespace TeamManager.Modules.Issues.Views
{
    public partial class TimelogFormView : IModalWindow
    {
        public TimelogFormView()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            TimeEntryForm.CommitEdit();
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            TimeEntryForm.CancelEdit();
            DialogResult = false;
        }
    }
}

