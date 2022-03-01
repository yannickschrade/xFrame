namespace xFrame.Core.ViewInjection
{
    public interface IDialogWindow
    {
        void Show();
        void ShowDialog();
        object Content { get; set; }
    }
}