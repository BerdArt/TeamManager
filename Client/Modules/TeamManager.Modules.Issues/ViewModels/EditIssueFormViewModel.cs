using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class EditIssueFormViewModel : NotificationObject
    {
        private bool _isNew;

        public Issue CurrentIssue { get; set; }
//        public ObservableCollection<IssueStatus> IssueStatuses { get; set; }

        public string WindowTitle { get; set; }

        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                if (_isNew)
                    WindowTitle = "Create new issue";
                else
                    WindowTitle = CurrentIssue != null
                                      ? string.Format("Edit issue '{0}'", CurrentIssue.Subject)
                                      : "Edit issue";
            }
        }

        public EditIssueFormViewModel(/*TeamManagerDomainContext context*/)
        {
            //Priorities = EnumHelper.GetValues<IssuePriority>();
            WindowTitle = "Edit issue";
            /*context.Load(
                context.GetIssueStatusesQuery(),
                loadOperation =>
                    {
                        IssueStatuses = new ObservableCollection<IssueStatus>(context.IssueStatus);
                        RaisePropertyChanged(() => IssueStatuses);
                    }, null);*/
        }
    }
}