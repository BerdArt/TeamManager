using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
//using TeamManager.Modules.Issues;
//using TeamManager.ViewModels;
//using TeamManager.Views;
using TeamManager.Infrastructure;
using TeamManager.Infrastructure.Messages;
using TeamManager.Infrastructure.ModalDialog;
using TeamManager.Web.Services;
using Modularity = Microsoft.Practices.Prism.Modularity;

namespace TeamManager
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();
            var busyIndicator = new Controls.BusyIndicator
            {
                Content = shell,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

            WebContext.Current.Authentication.LoggedIn += PublishUserRoles;
            WebContext.Current.Authentication.LoggedOut += PublishUserRoles;

            Application.Current.RootVisual = busyIndicator;
            return busyIndicator;
        }

        public void PublishUserRoles(object sender, AuthenticationEventArgs args)
        {
            UserRoleService.GetInstance().UserRoles = new ObservableCollection<string>(
                WebContext.Current.User.Roles
                );
            /* Messanger.Get<UserLoginMessage>().Publish(
                       new UserLoginEventArgs
                       {
                           UserName = WebContext.Current.User.DisplayName,
                           UserRoles = new List<string>(WebContext.Current.User.Roles)
                       });*/
        }

        protected override IUnityContainer CreateContainer()
        {

            var container = base.CreateContainer();
            container.RegisterType<TeamManagerDomainContext>();
            container.RegisterType<IModalDialogService, ModalDialogService>();
            container.RegisterType<IMessageBoxService, MessageBoxService>();
//            container.RegisterType<LoginFormViewModel>();
//            container.RegisterType<LoginFormView>();
            return container;
        }
        
        protected override Modularity.IModuleCatalog CreateModuleCatalog()
        {
            return Modularity.ModuleCatalog.CreateFromXaml(
                    new Uri("TeamManager;component/ModulesCatalog.xaml", UriKind.Relative));
        }
    }
}