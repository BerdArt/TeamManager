using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using TeamManager.Web.Models;

namespace TeamManager.Modules.Projects.ViewModels
{
    public class EditProjectViewModel : NotificationObject
    {
        private string WindowTitle { get; set; }
        private string FormTitle { get; set; }
        private bool IsNew { get; set; }
        private Project _project;

        public Project CurrentProject
        {
            get { return _project; }
            set
            {
                _project = value;
                IsNew = _project.Id == 0;
                SetTitle();
                RaisePropertyChanged(() => CurrentProject);
            }
        }

        private void SetTitle()
        {
            if (IsNew)
            {
                WindowTitle = "Create new project";
                FormTitle = "New project";
            }
            else
            {
                WindowTitle = "Edit project";
                FormTitle = string.Format("Edit project '{0}'", CurrentProject.Title);
            }
            RaisePropertyChanged(() => WindowTitle);
            RaisePropertyChanged(() => FormTitle);
        }

        public EditProjectViewModel()
        {
            
        }
    }
}