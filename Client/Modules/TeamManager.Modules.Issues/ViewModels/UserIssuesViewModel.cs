using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class UserIssuesViewModel : NotificationObject
    {
        public List<Issue> Issues { get; set; }
        public string HeaderTitle { get; set; }

        public UserIssuesViewModel(TeamManagerDomainContext context)
        {
            context.Load(context.GetIssuesQuery(),
                         loadOperation =>
                             {
                                 Issues = new List<Issue>(context.Issues);
                                 RaisePropertyChanged("Issues");
                             }, null);
            HeaderTitle = "My issues";
        }
    }
}