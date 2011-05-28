using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace TeamManager.Infrastructure
{
    public static class RegionManagerUtils
    {
        public static void RegisterViewWithRegionInIndex(this IRegionManager regionManager, string regionName, Type viewType, int index)
        {
            var mainRegion = regionManager.Regions[regionName];
            var viewsAmount = mainRegion.Views.Count();
            if (index > viewsAmount)
            {
                regionManager.RegisterViewWithRegion(regionName, viewType);
//                mainRegion.Activate(mainRegion.Views.First());
                return;
            }

            if (index < 0)
                index = 0;

            // Save reference to each view existing in the RegionManager after the index to insert.
            var views = mainRegion.Views.Skip(index - 1).ToList();

            //Remove elements from region that are after index to insert.
            foreach (var t in views)
                mainRegion.Remove(t);

            //Register view in index to insert.
            regionManager.RegisterViewWithRegion(regionName, viewType);

            // Adding previously removed views
            views.ForEach(view => mainRegion.Add(view));

            if (mainRegion.Views.Count() > 0)
                mainRegion.Activate(mainRegion.Views.First());
        }
    }
}