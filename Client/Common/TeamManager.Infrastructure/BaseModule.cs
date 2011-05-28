using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace TeamManager.Infrastructure
{
    public abstract class BaseModule : IModule
    {
        protected IUnityContainer Container;

        protected BaseModule(IUnityContainer container)
        {
            Container = container;
        }

        public virtual void Initialize()
        {
            RegisterTypes();
        }

        protected abstract void RegisterTypes();
    }
}