using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class TimelogFormViewModel : NotificationObject
    {
        private TimeEntry _timeEntry;
        public ObservableCollection<Dictionary> Activities { get; set; }

        public TimeEntry CurrentLogEntry
        {
            get { return _timeEntry; }
            set
            {
                _timeEntry = value;
                RaisePropertyChanged(() => CurrentLogEntry);
            }
        }

        private string _issueTitle = "New time log entry";
        public string IssueTitle
        {
            get { return _issueTitle; }
            set
            {
                _issueTitle = value;
                RaisePropertyChanged(() => IssueTitle);
            }
        }

        public TimelogFormViewModel(IUnityContainer container)
        {
            var context = container.Resolve<TeamManagerDomainContext>("TM_DB");
            context.Dictionaries.Clear();
            context.Load(
                context.GetActivitiesQuery(), LoadBehavior.RefreshCurrent,
                loadOperation =>
                    {
                        Activities = new ObservableCollection<Dictionary>(context.Dictionaries);
                        RaisePropertyChanged(() => Activities);
                    },
                null
                );
        }

    }
}