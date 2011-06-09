using System.Windows;
using System.Windows.Controls;
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
//            ProjectForm.CommitEdit();
//            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
//            ProjectForm.CancelEdit();
//            DialogResult = false;
        }

        private void ProjectForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            switch (e.EditAction)
            {
                case DataFormEditAction.Cancel:
                    DialogResult = false;
                    break;
                case DataFormEditAction.Commit:
                    DialogResult = true;
                    break;
            }
        }
    }
}

