using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TeamManager.Modules.Issues.ViewModels;

namespace TeamManager.Modules.Issues.Views
{
    public partial class UserIssuesView : UserControl
    {
        public UserIssuesView()
        {
            InitializeComponent();
        }

        [Dependency]
        public UserIssuesViewModel ViewModel { set { DataContext = value; } }
    }
}
