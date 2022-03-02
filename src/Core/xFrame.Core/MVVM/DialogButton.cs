using System.Windows.Input;

namespace xFrame.Core.MVVM
{
    public class DialogButton
    {
        public object Content { get; set; }
        public ICommand Command { get; set; }
        public double Width { get; set; }

        public DialogButton(object content, ICommand command, double width = 50)
        {
            Content = content;
            Command = command;
            Width = width;
        }
    }
}