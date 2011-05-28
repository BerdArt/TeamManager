using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TeamManager.Modules.Issues.ViewModels;

namespace TeamManager.Modules.Issues.Views
{
//    [ViewSortHint("200")]
    public partial class ProjectIssuesView : UserControl
    {
        public ProjectIssuesView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ProjectIssuesViewModel ViewModel { set { DataContext = value; } }

    }
}
