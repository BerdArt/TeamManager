using Microsoft.Practices.Prism.ViewModel;

namespace TeamManager.Modules.Issues.Model
{
    public class GroupItem : NotificationObject
    {
        private string _display;
        public string Display
        {
            get { return _display; }
            set { _display = value; RaisePropertyChanged(() => Display); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(() => Value);}
        }

        public GroupItem(string displayName, string value)
        {
            Display = displayName;
            Value = value;
        }
    }
}