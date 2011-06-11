using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class UserIssuesViewModel : NotificationObject
    {
        private readonly IUnityContainer _container;
        public ObservableCollection<Issue> Issues { get; set; }
        public string HeaderTitle { get; set; }

        public UserIssuesViewModel(IUnityContainer container)
        {
            _container = container;
            var context = _container.Resolve<TeamManagerDomainContext>("TM_DB");
            context.Load(context.GetIssuesQuery(),
                         loadOperation =>
                             {
                                 Issues = new ObservableCollection<Issue>(context.Issues);
                                 RaisePropertyChanged("Issues");
                             }, null);
            HeaderTitle = "My issues";
        }
    }
}