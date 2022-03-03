using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace xFrame.WPF.Theming
{
    public abstract class Theme : ResourceDictionary
    {
        public abstract ThemeType ThemeType { get; }
        public abstract string ThemeName { get; }


        public Theme()
        {
            
        }
    }
}
