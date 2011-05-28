using System;

namespace TeamManager.Infrastructure.ModalDialog
{
    public class ModalDialogService : IModalDialogService
    {
        #region Implementation of IModalDialogService

        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel, Action<TDialogViewModel> onDialogClose)
        {
            view.DataContext = viewModel;
            if (onDialogClose != null)
            {
                view.Closed += (s, e) => onDialogClose(viewModel);
            }
            view.Show();
        }

        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel)
        {
            ShowDialog(view, viewModel, null);
        }

        #endregion
    }
}