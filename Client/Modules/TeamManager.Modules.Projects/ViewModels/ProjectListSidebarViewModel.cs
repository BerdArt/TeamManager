using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using TeamManager.Infrastructure.Messages;
using TeamManager.Web.Models;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Projects.ViewModels
{
    public class ProjectListSidebarViewModel : NotificationObject
    {
        private Project _selectedProject;
        private IRegionManager _regionManager;

        public ProjectListSidebarViewModel(IUnityContainer container)
        {
            _regionManager = container.Resolve<IRegionManager>();
            var context = container.Resolve<TeamManagerDomainContext>("TM_DB");

            context.Load(context.GetProjectsQuery(),
                                 loadOperation =>
                                     {
                                         Projects = new ObservableCollection<Project>(context.Projects);
                                         RaisePropertyChanged(() => Projects);
                                     }, null);

            HeaderTitle = "Project List";

            Messanger.Get<ProjectSelectionMessage>().Subscribe(OnProjectSelected);
        }

        public void OnProjectSelected(SelectedProjectArgs args)
        {
            Projects.ForEach(project =>
                                 {
                                     if (project.Id == args.ProjectId)
                                     {
                                         if (SelectedProject != project)
                                         {
                                             SelectedProject = project;
                                             RaisePropertyChanged(() => SelectedProject);
                                         }
                                         return;
                                     }
                                 });
        }

        public ObservableCollection<Project> Projects { get; set; }
        public string HeaderTitle { get; set; }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                Messanger.Get<ProjectSelectionMessage>().Publish(new SelectedProjectArgs
                                                                     {
                                                                         ProjectId = _selectedProject.Id,
                                                                         ProjectTitle = _selectedProject.Title
                                                                     });
                _regionManager.RequestNavigate("MainRegion", new Uri("ProjectIssuesView", UriKind.Relative));
            }
        }
    }
}