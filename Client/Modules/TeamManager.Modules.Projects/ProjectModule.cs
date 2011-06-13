using System;
using System.Windows.Media;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using TeamManager.Infrastructure;
using TeamManager.Infrastructure.Messages;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Modules.Projects.ViewModels;
using TeamManager.Modules.Projects.Views;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Projects
{
    [ViewSortHint("1")]
    public class ProjectModule : BaseRegionModule
    {
        public ProjectModule(IUnityContainer container) : base(container)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Messanger.Get<ActivityMessage>().Publish(new ActivityMessageArgs
            {
                Author = "BerdArt",
                Color = Colors.Blue,
                DateTime = DateTime.Now.AddHours(-2),
                Title = "Project 1 has been added",
                Type = "Project"
            });

            Messanger.Get<ActivityMessage>().Publish(new ActivityMessageArgs
            {
                Author = "BerdArt",
                Color = Colors.Cyan,
                DateTime = DateTime.Now,
                Title = "Project 2 has been added",
                Type = "Project"
            });

        }

        public override void RegisterRegions()
        {
            RegionManager.RegisterViewWithRegion("HeaderRegion", typeof(ProjectListSidebarView));
            RegionManager.RegisterViewWithRegionInIndex("MainRegion", typeof (ProjectListView), 1 );
//            RegionManager.RegisterViewWithRegion("MainRegion", typeof (ProjectListView));
        }

        protected override void RegisterTypes()
        {
            Container.RegisterType<TeamManagerDomainContext>(new InjectionConstructor());
            Container.RegisterType<ProjectListSidebarViewModel>();
            Container.RegisterType<ProjectListSidebarView>();
            Container.RegisterType<ProjectListViewModel>();
            Container.RegisterType<ProjectListView>();
            Container.RegisterType<EditProjectViewModel>();
            Container.RegisterType<IModalWindow, EditProjectView>("editprojectwindow");
        }

    }
}