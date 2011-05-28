using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using TeamManager.Infrastructure;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Modules.Issues.ViewModels;
using TeamManager.Modules.Issues.Views;
using TeamManager.Web.Services;

namespace TeamManager.Modules.Issues
{
    public class IssueModule : BaseRegionModule
    {

        #region Overrides of BaseModule

        public IssueModule(IUnityContainer container) : base(container)
        {
        }

        protected override void RegisterTypes()
        {
            Container.RegisterType<TeamManagerDomainContext>(new InjectionConstructor());
            Container.RegisterType<UserIssuesViewModel>();
            Container.RegisterType<UserIssuesView>();
            Container.RegisterType<IssueViewModel>();
            Container.RegisterType<IssueView>();
            Container.RegisterType<ProjectIssuesViewModel>();
            Container.RegisterType<ProjectIssuesView>();
            Container.RegisterType<EditIssueFormViewModel>();
            Container.RegisterType<TimelogFormViewModel>();
            Container.RegisterType<IModalWindow, TimelogFormView>("timelog_form");
        }

        #endregion

        #region Overrides of BaseRegionModule

        public override void RegisterRegions()
        {
//            RegionManager.RegisterViewWithRegion("RightSidebar", typeof (UserIssuesView));
            RegionManager.RegisterViewWithRegionInIndex("MainRegion", typeof (ProjectIssuesView), 2);
            RegionManager.RegisterViewWithRegionInIndex("MainRegion", typeof (IssueView), 3);
        }

        #endregion
    }
}