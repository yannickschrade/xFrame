using System.Windows.Media;
using xFrame.WPF.Theming.Generators;

namespace xFrame.WPF.Theming.Templates
{
    [GenerateThemeTemplate]
    public abstract partial class DefaultThemeTemplate : Theme
    {
        
        private Color _baseColor;
        private Color _baseColor02;
        private Color _baseColor05;
        private Color _baseColor10;
        private Color _baseColor20;
        private Color _baseColor30;
        private Color _baseColor40;
        private Color _baseColor50;
        private Color _baseColor60;
        private Color _baseColor70;
        private Color _baseColor80;
        private Color _baseColor90;

        private Color _disabledColor;

        private Color _accentColor;
        private Color _foregroundColor;
        private Color _foregroundDisabeldColor;
        private Color _backgroundColor;
        private Color _hoverColor;
        private Color _buttonPressedColor;
        private Color _borderColor;
        private Color _controlBackgroundColor;

        [DefaultValue(10)]
        private double _fontSizeXXSmall;
        
        [DefaultValue(12)]
        private double _fontSizeXSmall;
        
        [DefaultValue(14)]
        private double _fontSizeSmall;

        [DefaultValue(16)]
        private double _fontSizeRegular;

        [DefaultValue(18)] 
        private double _fontSizeLarge;
        
        [DefaultValue(22)] 
        private double _fontSizeXLarge;
        
        [DefaultValue(26)] 
        private double _fontSizeXXLarge;
    }
}
