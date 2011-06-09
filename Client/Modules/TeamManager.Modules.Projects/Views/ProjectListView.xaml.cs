using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using TeamManager.Modules.Projects.ViewModels;
using System.ComponentModel;
using TeamManager.Web.Models;

namespace TeamManager.Modules.Projects.Views
{
//    [ViewSortHint("100")]
    public partial class ProjectListView : UserControl
    {
        public ProjectListView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ProjectListViewModel ViewModel { set { DataContext = value;  } }
    }
}
