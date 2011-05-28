using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TeamManager.Modules.Issues.ViewModels;

namespace TeamManager.Modules.Issues.Views
{
    public partial class IssueView : UserControl
    {
        public IssueView()
        {
            InitializeComponent();
        }

        [Dependency]
        public IssueViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}
