using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;
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
using TeamManager.Modules.Issues.Model;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class ProjectIssuesViewModel: NotificationObject
    {
        public string HeaderTitle { get; set; }
        public PagedCollectionView Issues { get; set; }
        public ObservableCollection<Issue> IssueList { get; set; }
        public ObservableCollection<GroupItem> GroupCriteria { get; set; }
        public ICommand EditIssueCommand { get; set; }
        public ICommand CreateIssueCommand { get; set; }
        public ICommand DeleteIssueCommand { get; set; }
        public ICommand OpenIssueCommand { get; set; }
        public ICommand GroupChangedCommand { get; set; }
        public ICommand ClearGroupingCommand { get; set; }
        

        int CurrentProjectId { get; set; }

        private readonly IUnityContainer _container;
        private readonly TeamManagerDomainContext _context;
        private readonly IModalDialogService _modalDialogService;
        private readonly IMessageBoxService _messageBoxService;

        public ProjectIssuesViewModel(IUnityContainer container)
        {
            HeaderTitle = "Issues";


            _container = container;
            _context = _container.Resolve<TeamManagerDomainContext>("TM_DB"); 
            _modalDialogService = _container.Resolve<IModalDialogService>();
            _messageBoxService = _container.Resolve<IMessageBoxService>();

            Messanger.Get<ProjectSelectionMessage>().Subscribe(OnSelectedProjectChanged);

            GroupCriteria = new ObservableCollection<GroupItem>(new List<GroupItem>
                                                                    {
                                                                        new GroupItem("Tracker", "Tracker.Name"),
                                                                        new GroupItem("Priority", "Priority.Name"),
                                                                        new GroupItem("Creator", "Creator.UserName"),
                                                                        new GroupItem("Assigned member", "AssignedUser.UserName"),
                                                                    });

            EditIssueCommand = new DelegateCommand<Issue>(ExecuteEditIssue, issue => true);
            CreateIssueCommand = new DelegateCommand(ExecuteCreateIssue, CanExecuteCreateIssue);
            DeleteIssueCommand = new DelegateCommand<Issue>(ExecuteDeleteIssue, issue => true);
            OpenIssueCommand = new DelegateCommand<Issue>(ExecuteIssueNavigate, issue => true);
            GroupChangedCommand = new DelegateCommand<SelectionChangedEventArgs>(GroupChangedHandler, e => true);
            ClearGroupingCommand = new DelegateCommand(ClearGroupingExecute, () => true);
        }

        private void ClearGroupingExecute()
        {
            Issues.GroupDescriptions.Clear();
        }

        private void GroupChangedHandler(SelectionChangedEventArgs e)
        {
            var field = (GroupItem) e.AddedItems[0];
            Issues.GroupDescriptions.Clear();
            var pgd = new PropertyGroupDescription(field.Value);
            Issues.GroupDescriptions.Add(pgd);
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
                                  IssueList = new ObservableCollection<Web.Models.Issue>(_context.Issues);
                                  Issues = new PagedCollectionView(IssueList);
                                  RaisePropertyChanged(() => Issues);
                              }, null);
        }
    }
}