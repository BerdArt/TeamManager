using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using TeamManager.Infrastructure.Messages;

namespace TeamManager.Infrastructure
{
    public class AccessByRoleBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            SetVisability();
        }

        protected override void OnDetaching()
        {
            _userRoles = null;
            base.OnDetaching();
        }

        #region AllowRoles

        /// <summary>
        /// AllowRoles Dependency Property
        /// </summary>
        public static readonly DependencyProperty AllowRolesProperty =
            DependencyProperty.Register("AllowRoles", typeof (string),
                                        typeof (AccessByRoleBehavior),
                                        new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the AllowRoles property.  This dependency property 
        /// indicates ....
        /// </summary>
        public string AllowRoles
        {
            get { return (string)GetValue(AllowRolesProperty); }
            set { SetValue(AllowRolesProperty, value); }
        }

        #endregion

        #region UserRoles

        /// <summary>
        /// UserRoles Dependency Property
        /// </summary>
        public static readonly DependencyProperty UserRolesProperty =
            DependencyProperty.Register("UserRoles", typeof (ObservableCollection<String>),
                                        typeof (AccessByRoleBehavior),
                                        new PropertyMetadata(null, OnUserRolesChanged));

        private string[] _userRoles;
        /// <summary>
        /// Gets or sets the UserRoles property.  This dependency property 
        /// indicates ....
        /// </summary>
        public ObservableCollection<String> UserRoles
        {
            get { return (ObservableCollection<String>)GetValue(UserRolesProperty); }
            set { SetValue(UserRolesProperty, value); }
        }

        /// <summary>
        /// Handles changes to the UserRoles property.
        /// </summary>
        private static void OnUserRolesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AccessByRoleBehavior)d).CheckVisibility(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the UserRoles property.
        /// </summary>
        protected void CheckVisibility(DependencyPropertyChangedEventArgs e)
        {
            var collection = (ObservableCollection<String>)e.NewValue;
            _userRoles = new string[collection.Count];
            for (var i = 0; i < collection.Count; i++)
            {
                _userRoles[i] = collection[i];
            }
            SetVisability();
        }

        protected void SetVisability()
        {
            if (AssociatedObject == null) return;

            if (_userRoles != null)
            {
                var allowRoles = AllowRoles.Split(new[] { ',' }).ToList();
                if (_userRoles.Any(allowRoles.Contains))
                {
                    if (AssociatedObject is DataForm)
                        (AssociatedObject as DataForm).CommandButtonsVisibility = 
                            DataFormCommandButtonsVisibility.All;
                    else
                        AssociatedObject.Visibility = Visibility.Visible;
                    return;
                }
            }
            if (AssociatedObject is DataForm)
                (AssociatedObject as DataForm).CommandButtonsVisibility = 
                    DataFormCommandButtonsVisibility.Navigation;
            else
                AssociatedObject.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}