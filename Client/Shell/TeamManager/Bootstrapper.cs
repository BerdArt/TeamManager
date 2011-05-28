using System;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
//using TeamManager.Modules.Issues;
//using TeamManager.ViewModels;
//using TeamManager.Views;
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

            Application.Current.RootVisual = busyIndicator;
            return busyIndicator;
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