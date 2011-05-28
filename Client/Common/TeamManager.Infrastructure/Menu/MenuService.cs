using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamManager.Infrastructure.Menu
{
    class MenuService : IMenuService
    {
        private readonly IList<MenuItemViewModel> _items = new List<MenuItemViewModel>();
        #region Implementation of IMenuService

        public void AddItem(string name, Action action)
        {
            _items.Add(new MenuItemViewModel {Name = name, Action = action});
        }

        public IEnumerable<MenuItemViewModel> Items { get { return _items.AsEnumerable();  } }

        #endregion
    }
}