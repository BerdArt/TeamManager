using System;

namespace TeamManager.Infrastructure.Menu
{
    public class MenuItemViewModel
    {
        public string Name { get; set; }
        public Action Action { get; set; }
    }
}