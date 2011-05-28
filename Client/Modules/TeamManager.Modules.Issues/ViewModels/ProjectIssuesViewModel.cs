using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;
using System.Windows.Data;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Infrastructure.Messages;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Infrastructure.Commands;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class ProjectIssuesViewModel: NotificationObject
    {
        public string HeaderTitle { get; set; }
        public PagedCollectionView Issues { get; set; }
        public ObservableCollection<Issue> IssueList { get; set; }
        public ICommand EditIssueCommand { get; set; }
        public ICommand CreateIssueCommand { get; set; }
        public ICommand DeleteIssueCommand { get; set; }
        public ICommand OpenIssueCommand { get; set; }
        

        int CurrentProjectId { get; set; }

        private readonly IUnityContainer _container;
        private readonly TeamManagerDomainContext _context;
        private readonly IModalDialogService _modalDialogService;
        private readonly IMessageBoxService _messageBoxService;

        public ProjectIssuesViewModel(TeamManagerDomainContext context, IUnityContainer container, IModalDialogService dialogService, IMessageBoxService messageService)
        {
            HeaderTitle = "Issues";
            

            _context = context;
            _container = container;
            _modalDialogService = dialogService;
            _messageBoxService = messageService;

            Messanger.Get<ProjectSelectionMessage>().Subscribe(OnSelectedProjectChanged);

            EditIssueCommand = new DelegateCommand<Issue>(ExecuteEditIssue, issue => true);
            CreateIssueCommand = new DelegateCommand(ExecuteCreateIssue, CanExecuteCreateIssue);
            DeleteIssueCommand = new DelegateCommand<Issue>(ExecuteDeleteIssue, issue => true);
            OpenIssueCommand = new DelegateCommand<Issue>(ExecuteIssueNavigate, issue => true);
        }

        public void ExecuteIssueNavigate(Issue issue)
        {
            if (issue.Id == 0) return; 
            var regionManager = _container.Resolve<RegionManager>();
            var query = new UriQuery {{"issue", issue.Id.ToString()}};
            regionManager.RequestNavigate("MainRegion", new Uri("IssueView" + query, UriKind.Relative));
        }

        public void OnSelectedProjectChanged(SelectedProjectArgs args)
        {
            CurrentProjectId = args.ProjectId;
            HeaderTitle = string.Format("Issues of '{0}'", args.ProjectTitle);
            RaisePropertyChanged(() => HeaderTitle);
            (CreateIssueCommand as DelegateCommand).UpdateCanExecute();
            LoadData();
        }

        public void ExecuteDeleteIssue(Issue issue)
        {
            var message = string.Format("Are you sure you want delete issue '{0}'", issue.Subject);
            var result = _messageBoxService.Show(message, "Confirm deleting", GenericMessageBoxButton.OkCancel);
            if (result == GenericMessageBoxResult.Cancel) return;
            _context.Issues.Remove(issue);
            _context.SubmitChanges();
            LoadData();
        }

        public void ExecuteCreateIssue()
        {
            /*var viewModel = _container.Resolve<EditIssueFormViewModel>();
            viewModel.CurrentIssue = new Issue {ProjectId = CurrentProjectId, Subject = "New issue", PriorityId = 4, Description = "Nothing" };
            viewModel.IsNew = true;
            var window = _container.Resolve<IModalWindow>("editissueform");
            _modalDialogService.ShowDialog(window, viewModel,
                formViewModel =>
                    {
                        if (!window.DialogResult.HasValue ||
                                                !window.DialogResult.Value) return;

                        var newIssue = formViewModel.CurrentIssue;
                        _context.Issues.Add(newIssue);
                        Issues.Add(newIssue);
                        _context.SubmitChanges();
                        RaisePropertyChanged("Issues");
                    }
                );*/
            var regionManager = _container.Resolve<RegionManager>();
            var query = new UriQuery { { "issue", "0" } };
            regionManager.RequestNavigate("MainRegion", new Uri("IssueView" + query, UriKind.Relative));
        }

        public bool CanExecuteCreateIssue()
        {
            return CurrentProjectId != 0;
        }

        public void ExecuteEditIssue(Issue issue)
        {
           /* var viewModel = _container.Resolve<EditIssueFormViewModel>();
            viewModel.CurrentIssue = issue;
            viewModel.IsNew = false;
            var window = _container.Resolve<IModalWindow>("editissueform");
            _modalDialogService.ShowDialog(window, viewModel,
                                           formViewModel =>
                                               {
                                                   if (!window.DialogResult.HasValue ||
                                                       !window.DialogResult.Value) return;

                                                   var index = Issues.IndexOf(issue);
                                                   Issues[index] = formViewModel.CurrentIssue;
                                                   _context.SubmitChanges();
                                                   RaisePropertyChanged("Issues");
                                               }
                );*/
        }

        private void LoadData()
        {
            var query = CurrentProjectId == 0
                            ? _context.GetIssuesQuery()
                            : _context.GetIssuesByProjectQuery(CurrentProjectId);
            _context.Load(query, LoadBehavior.RefreshCurrent,
                          loadOperation =>
                              {
                                  IssueList = new ObservableCollection<Issue>(_context.Issues);
                                  Issues = new PagedCollectionView(IssueList);
                                  RaisePropertyChanged(() => Issues);
                              }, null);
        }
    }

}