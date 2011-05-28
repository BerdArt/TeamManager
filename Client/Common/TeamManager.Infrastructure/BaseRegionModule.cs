using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace TeamManager.Infrastructure
{
    public abstract class BaseRegionModule : BaseModule
    {
        protected IRegionManager RegionManager;
        protected BaseRegionModule(IUnityContainer container) : base(container)
        {
            RegionManager = (IRegionManager) container.Resolve(typeof (IRegionManager));
        }

        public override void Initialize()
        {
            base.Initialize();
            RegisterRegions();
        }

        public abstract void RegisterRegions();
    }
}