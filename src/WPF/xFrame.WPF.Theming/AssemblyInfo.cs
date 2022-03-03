using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsDefinition("xFrame:Wpf.Theming", "xFrame.WPF.Theming")]
[assembly: XmlnsDefinition("xFrame:Wpf.Theming", "xFrame.WPF.Theming.Themes")]
[assembly: XmlnsDefinition("xFrame:Wpf.Theming", "xFrame.WPF.Theming.Templates")]
[assembly: XmlnsDefinition("xFrame:Wpf.Theming.Fluent", "xFrame.WPF.Theming.Themes.Fluent")]
[assembly: XmlnsDefinition("xFrame:Wpf.Theming.Fluent", "xFrame.WPF.Theming.Templates")]
