using System;

namespace TeamManager.Infrastructure.ModalDialog
{
    public interface IModalDialogService
    {
        void ShowDialog<TViewModel>(IModalWindow view, TViewModel viewModel, Action<TViewModel> onDialogClose);
        void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel);
    }
}