using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Infrastructure;
using TeamManager.Infrastructure.Messages;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Projects.ViewModels
{
    public class ProjectListViewModel : NotificationObject 
    {
        public ObservableCollection<Project> Projects { get; set; }
        public string HeaderTitle { get; set; }

        public ICommand CreateProjectCommand { get; set; }
        public ICommand EditProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand ViewProjectCommand { get; set; }

        private readonly TeamManagerDomainContext _context;
        private readonly IUnityContainer _container;
        private readonly IModalDialogService _modalDialogService;
        private readonly IMessageBoxService _messageBoxService;

        public ProjectListViewModel(TeamManagerDomainContext context, IUnityContainer container, IModalDialogService modalDialogService, IMessageBoxService messageBoxService)
        {
            HeaderTitle = "Project List";
            _context = context;
            _container = container;
            _modalDialogService = modalDialogService;
            _messageBoxService = messageBoxService;
            CreateProjectCommand = new DelegateCommand(CreateProjectExecute, () => true);
            EditProjectCommand = new DelegateCommand<Project>(EditProjectExecute, project => true);
            DeleteProjectCommand = new DelegateCommand<Project>(DeleteProjectExecute, project => true);
            ViewProjectCommand = new DelegateCommand<Project>(ViewProjectExecute, project => true);
            UserRoleService.GetInstance().PropertyChanged += UserRolesChanged;
            LoadData();
        }

        private void UserRolesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserRoles")
                LoadData();
        }

        public void LoadData()
        {
            var query = UserRoleService.GetInstance().UserRoles.Count > 0
                            ? _context.GetProjectsQuery()
                            : _context.GetPublicProjectsQuery();
            
            _context.Load(query, LoadBehavior.RefreshCurrent,
                         loadOperation =>
                         {
                             Projects = new ObservableCollection<Project>(_context.Projects);
                             RaisePropertyChanged(() => Projects);
                         }, null);
        }

        public void CreateProjectExecute()
        {
            var window =
                _container.Resolve<IModalWindow>("editprojectwindow");
            var viewModel =
                _container.Resolve<EditProjectViewModel>();
            viewModel.CurrentProject = new Project();
            _modalDialogService.ShowDialog(window, viewModel,
                returnedViewModelInstance =>
                {
                    if (!window.DialogResult.HasValue ||
                        !window.DialogResult.Value) return;

                    _context.Projects.Add(returnedViewModelInstance.CurrentProject);
                    _context.SubmitChanges();
                    LoadData();
                });
        }

        public void EditProjectExecute(Project project)
        {
            var window =
                _container.Resolve<IModalWindow>("editprojectwindow");
            var viewModel =
                _container.Resolve<EditProjectViewModel>();
            viewModel.CurrentProject = project; 
            _modalDialogService.ShowDialog(window, viewModel,
                returnedViewModelInstance =>
                {
                    if (!window.DialogResult.HasValue ||
                        !window.DialogResult.Value) return;

                    var index = Projects.IndexOf(project);
                    Projects[index] = returnedViewModelInstance.CurrentProject;
//                    _context.Projects.Detach(project);
                    _context.SubmitChanges();
                    LoadData();
                });
        }

        public void DeleteProjectExecute(Project project)
        {
            var result =
                _messageBoxService.Show(string.Format("Are you sure you want to delete project '{0}'?", project.Title),
                                        "Deleting object", GenericMessageBoxButton.OkCancel);
            if (result == GenericMessageBoxResult.Cancel) return;

            _context.Projects.Remove(project);
            _context.SubmitChanges();
            LoadData();
        }

        public void ViewProjectExecute(Project project)
        {
            Messanger.Get<ProjectSelectionMessage>().Publish(new SelectedProjectArgs
            {
                ProjectId = project.Id,
                ProjectTitle = project.Title
            });
            var regionManager = _container.Resolve<RegionManager>();
            regionManager.RequestNavigate("MainRegion", new Uri("ProjectIssuesView", UriKind.Relative));

        }
    }
}