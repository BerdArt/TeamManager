using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using TeamManager.Web.Models;

namespace TeamManager.Modules.Projects.ViewModels
{
    /// <summary>
    /// Model for EditProject view. Store WindowTitle and CurrentProject.
    /// </summary>
    public class EditProjectViewModel : NotificationObject
    {
        private Project _project;

        /// <summary>
        /// Window title
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Current project property. Store information of the currently edited project.
        /// Binds to DataForm.
        /// </summary>
        public Project CurrentProject
        {
            get { return _project; }
            set
            {
                _project = value;
                WindowTitle = _project.Id == 0 ? "New project" : "Edit project";
                RaisePropertyChanged(() => WindowTitle);
                RaisePropertyChanged(() => CurrentProject);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditProjectViewModel()
        {
            WindowTitle = "Edit project form";
        }
    }
}