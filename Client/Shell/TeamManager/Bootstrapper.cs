using System;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
//using TeamManager.Infrastructure.ModalDialog;
//using TeamManager.Modules.Issues;
//using TeamManager.Services.Web.Services;
//using TeamManager.ViewModels;
//using TeamManager.Views;
using Modularity = Microsoft.Practices.Prism.Modularity;

namespace TeamManager
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = new Shell();
            Application.Current.RootVisual = shell;
            return shell;
        }

        protected override IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
//            container.RegisterType<TeamManagerDomainContext>();
//            container.RegisterType<IModalDialogService, ModalDialogService>();
//            container.RegisterType<IMessageBoxService, MessageBoxService>();
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