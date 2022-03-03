using System.Windows.Media;
using xFrame.WPF.Theming.ExtensionMethodes;
using xFrame.WPF.Theming.Templates;

namespace xFrame.WPF.Theming.Themes
{
    public class DarkTheme : DefaultThemeTemplate
    {
        public override ThemeType ThemeType => ThemeType.Dark;
        public override string ThemeName => "DefaultDark";
        public override Color BaseColor => (Color)ColorConverter.ConvertFromString("#202020");
        public override Color BaseColor02 => BaseColor.Lighten(2);
        public override Color BaseColor05 => BaseColor.Lighten(5);
        public override Color BaseColor10 => BaseColor.Lighten(10);
        public override Color BaseColor20 => BaseColor.Lighten(20);
        public override Color BaseColor30 => BaseColor.Lighten(30);
        public override Color BaseColor40 => BaseColor.Lighten(40);
        public override Color BaseColor50 => BaseColor.Lighten(50);
        public override Color BaseColor60 => BaseColor.Lighten(60);
        public override Color BaseColor70 => BaseColor.Lighten(70);
        public override Color BaseColor80 => BaseColor.Lighten(80);
        public override Color BaseColor90 => BaseColor.Lighten(90);


        public override Color AccentColor => ThemeHelper.GetAccentColor();
        public override Color ForegroundColor => Colors.White;
        public override Color ForegroundDisabeldColor => ForegroundColor.Darken(50);
        public override Color BackgroundColor => BaseColor;
        public override Color DisabledColor => BaseColor20;

        public override Color HoverColor => BaseColor10;
        public override Color ButtonPressedColor => BaseColor02;
        public override Color BorderColor => BaseColor05;

        public override Color ControlBackgroundColor => BaseColor05;

    }
}
