using System;
using Microsoft.Practices.Prism.Modularity;

namespace TeamManager
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using TeamManager.LoginUI;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class Shell : UserControl
    {
        private readonly IModuleManager _moduleManager;

        /// <summary>
        /// Creates a new <see cref="Shell"/> instance.
        /// </summary>
        public Shell(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
            InitializeComponent();
        }

        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            LoadModule(e.Uri.ToString());
            foreach (var child in LinksStackPanel.Children)
            {
                var hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    VisualStateManager.GoToState(hb,
                                                 hb.NavigateUri.ToString().Equals(e.Uri.ToString())
                                                     ? "ActiveLink"
                                                     : "InactiveLink", true);
                }
            }
        }

        private void LoadModule(string uri)
        {
            if (!ModuleMapper.ModuleMap.ContainsKey(uri)) return;
            ModuleMapper.ModuleMap[uri].ForEach(module => _moduleManager.LoadModule(module));
        }

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}