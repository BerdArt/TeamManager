using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class TimelogFormViewModel : NotificationObject
    {
        private bool _isNew = false;
        private TimeEntry _timeEntry;
        public ObservableCollection<Dictionary> Activities { get; set; }

        public TimeEntry CurrentLogEntry
        {
            get { return _timeEntry; }
            set
            {
                _timeEntry = value;
                _timeEntry.SpentOn = DateTime.Now;
                RaisePropertyChanged(() => CurrentLogEntry);
            }
        }

        private Issue _issue;
        public Issue Issue
        {
            get { return _issue; }
            set
            {
                _issue = value;
                IssueTitle = _issue.Subject;
                RaisePropertyChanged(() => IssueTitle);
            }
        }

        public string IssueTitle { get; set; }

        public TimelogFormViewModel(IUnityContainer container)
        {
            var context = container.Resolve<TeamManagerDomainContext>();

            context.Load(
                context.GetActivitiesQuery(),
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