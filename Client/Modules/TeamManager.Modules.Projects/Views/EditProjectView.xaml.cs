using System.Windows;
using TeamManager.Infrastructure.ModalDialog;

namespace TeamManager.Modules.Projects.Views
{
    public partial class EditProjectView : IModalWindow
    {
        public EditProjectView()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectForm.CommitEdit();
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectForm.CancelEdit();
            DialogResult = false;
        }
    }
}

