using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using TeamManager.Infrastructure.Messages;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Web.Models;
using TeamManager.Web.Services;
using Microsoft.Practices.Prism.ViewModel;

namespace TeamManager.Modules.Issues.ViewModels
{
    public class IssueViewModel : NotificationObject, INavigationAware
    {
        private readonly TeamManagerDomainContext _context;
        private readonly IUnityContainer _container;
        private bool _isNewItem;
        public string HeaderTitle { get; set; }
        public int CurrentProjectId { get; set; }
        public bool AutoEditAllowed { get; set; }
        public ObservableCollection<Issue> IssueList { get; set; }
        public ObservableCollection<IssueStatus> IssueStatuses { get; set; }
        public ObservableCollection<Tracker> Trackers { get; set; }
        public ObservableCollection<Dictionary> Priorities { get; set; }
        public PagedCollectionView Issues { get; set; }
        public ICommand SaveIssueCommand { get; set; }
        public ICommand ShowIssuesCommand { get; set; }
        public ICommand AddingNewItemEvent { get; set; }
        public ICommand LogTimeCommand { get; set; }

        public IssueViewModel(IUnityContainer container)
        {
            _container = container;
            _context = _container.Resolve<TeamManagerDomainContext>("TM_DB");
            HeaderTitle = "Issue";
            AutoEditAllowed = false;

            Messanger.Get<ProjectSelectionMessage>().Subscribe(OnSelectedProjectChanged);
            
            _context.Load(
                _context.GetIssueStatusesQuery(), LoadBehavior.RefreshCurrent,
                loadOperation =>
                    {
                        IssueStatuses = new ObservableCollection<IssueStatus>(_context.IssueStatus);
                        RaisePropertyChanged(() => IssueStatuses);
                    },
                null);
            _context.Load(
                _context.GetTrackersQuery(), LoadBehavior.RefreshCurrent,
                loadOperation =>
                    {
                        Trackers = new ObservableCollection<Tracker>(_context.Trackers);
                        RaisePropertyChanged(() => Trackers);
                    },
                null
                );
            _context.Load(
                _context.GetPrioritiesQuery(), LoadBehavior.RefreshCurrent,
                loadOperation =>
                    {
                        Priorities = new ObservableCollection<Dictionary>(_context.Dictionaries);
                        RaisePropertyChanged(() => Priorities);
                    },
                null
                );

            SaveIssueCommand = new DelegateCommand<DataFormEditEndedEventArgs>(ExecuteSaveIssue, a => true);
            ShowIssuesCommand = new DelegateCommand(ExecuteShowIssues, ()=> true);
            AddingNewItemEvent = new DelegateCommand(AddingNewItemEventHandler, () => true);
            LogTimeCommand = new DelegateCommand(LogTimeExecute, () => true);
        }

        public void LogTimeExecute()
        {
            var viewModel = _container.Resolve<TimelogFormViewModel>();
            viewModel.CurrentLogEntry = new TimeEntry { SpentOn = DateTime.Now };
            
            viewModel.IssueTitle = ((Issue) Issues.CurrentItem).Subject;

            var view = _container.Resolve<IModalWindow>("timelog_form");
            var modalDialogService = _container.Resolve<IModalDialogService>();
            modalDialogService.ShowDialog(
                view, viewModel,
                returnedViewModel =>
                    {
                        if (!view.DialogResult.HasValue || !view.DialogResult.Value)
                            return;
                        viewModel.CurrentLogEntry.Issue = Issues.CurrentItem as Issue;
                        
                        _context.TimeEntries.Add(viewModel.CurrentLogEntry);
                        _context.SubmitChanges();
                        RaisePropertyChanged(() => ((Issue) Issues.CurrentItem).SpendedTime);
                    });
        }

        private void OnCurrentChanged(object sender, EventArgs e)
        {
            var issue = Issues.CurrentItem as Issue;
            if (issue == null) return;
            if (issue.Id == 0)
            {
                HeaderTitle = "New issue";
                AutoEditAllowed = true;
            } 
            else
            {
                HeaderTitle = string.Format("{0} #{1}", issue.Tracker.Name, issue.Id);
                AutoEditAllowed = false;
            }
            RaisePropertyChanged(() => AutoEditAllowed);
            RaisePropertyChanged(() => HeaderTitle);
        }

        #region Command execute handlers
        private void AddingNewItemEventHandler()
        {
            _isNewItem = true;
        }

        private void ExecuteShowIssues()
        {
            var regionMananger = _container.Resolve<RegionManager>();
            regionMananger.RequestNavigate("MainRegion", new Uri("ProjectIssuesView", UriKind.Relative));
        }

        public void ExecuteSaveIssue(DataFormEditEndedEventArgs args)
        {
            var issue = Issues.CurrentItem as Web.Models.Issue;
            if (args.EditAction == DataFormEditAction.Cancel)
            {
                _context.RejectChanges();
                if (_isNewItem)
                    Issues.MoveCurrentToPrevious();
                return;
            }
            issue.ProjectId = CurrentProjectId;
            if (_context.HasChanges)
            {
                _context.SubmitChanges();
            }
            _isNewItem = false;
            AutoEditAllowed = false;
            RaisePropertyChanged(() => AutoEditAllowed);
        }
        #endregion

        public void OnSelectedProjectChanged(SelectedProjectArgs args)
        {
            CurrentProjectId = args.ProjectId;
            _context.Issues.Clear();
            LoadData();
        }

        private void LoadData()
        {
            _context.Load(
                _context.GetIssuesByProjectQuery(CurrentProjectId), LoadBehavior.RefreshCurrent,
                loadOperation =>
                    {
                        IssueList = new ObservableCollection<Issue>(_context.Issues);
                        Issues = new PagedCollectionView(IssueList);
                        Issues.CurrentChanged += OnCurrentChanged;
                        AutoEditAllowed = false;
                        RaisePropertyChanged(() => AutoEditAllowed);
                        RaisePropertyChanged(() => Issues);
                    },
                null);

        }

        #region Implementation of INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var issueId = int.Parse(navigationContext.Parameters["issue"]);
            if (Issues == null) return;
            if (issueId == 0)
            {
                Issues.MoveCurrentTo(Issues.AddNew());
//                Issues.EditItem(Issues.CurrentItem);
            }
            else
                Issues.MoveCurrentTo(IssueList.FirstOrDefault(issue => issue.Id == issueId));
            OnCurrentChanged(null, null);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return navigationContext.Parameters["issue"] != null;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion
    }
}