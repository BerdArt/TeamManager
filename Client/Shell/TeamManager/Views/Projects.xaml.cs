using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;

namespace TeamManager
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Projects page for the application.
    /// </summary>
    public partial class Projects : Page
    {
        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Projects()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.ProjectsPageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}