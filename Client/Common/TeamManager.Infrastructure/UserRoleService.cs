using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel;

namespace TeamManager.Infrastructure
{
    public class UserRoleService : NotificationObject
    {
        private static UserRoleService instance;

        public static UserRoleService GetInstance()
        {
            if (instance == null)
            {
                instance = new UserRoleService(); ;
            }
            return instance;
        }

        private UserRoleService()
        {
            _userRoles = new ObservableCollection<string>();
        }

        private ObservableCollection<string> _userRoles;
        public ObservableCollection<string> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                RaisePropertyChanged(() => UserRoles);
            }
        }
    }

    public class UserRolesProxy
    {
        public virtual UserRoleService RolesService
        {
            get { return DesignerProperties.IsInDesignTool ? null : UserRoleService.GetInstance(); }
        }
    }
}