using System;
using System.Collections.Generic;

namespace TeamManager.Infrastructure.Menu
{
    public interface IMenuService
    {
        void AddItem(string name, Action action);
        IEnumerable<MenuItemViewModel> Items { get; }
    }
}