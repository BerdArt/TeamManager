using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TeamManager.Modules.Projects.ViewModels;

namespace TeamManager.Modules.Projects.Views
{
    public partial class ProjectListSidebarView : UserControl
    {
        public ProjectListSidebarView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ProjectListSidebarViewModel ViewModel { set { DataContext = value; } }
    }
}
