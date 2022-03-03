using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// In Projekten im SDK-Stil wie dem vorliegenden, bei dem verschiedene Assemblyattribute
// üblicherweise in dieser Datei definiert wurden, werden diese Attribute jetzt während
// der Builderstellung automatisch hinzugefügt und mit Werten aufgefüllt, die in den
// Projekteigenschaften definiert sind. Informationen dazu, welche Attribute einbezogen
// werden und wie dieser Prozess angepasst werden kann, finden Sie unter https://aka.ms/assembly-info-properties.


// Wenn "ComVisible" auf FALSE festgelegt wird, sind die Typen in dieser Assembly
// für COM-Komponenten nicht sichtbar. Wenn Sie von COM aus auf einen Typ in dieser
// Assembly zugreifen müssen, legen Sie das ComVisible-Attribut für den betreffenden
// Typ auf TRUE fest.

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]


[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM
// bereitgestellt wird.

[assembly: Guid("cf7ef51a-613f-4558-b353-dd9307cd31bb")]

[assembly: XmlnsDefinition("xFrame:Wpf", "xFrame.WPF.Controls")]
[assembly: XmlnsDefinition("xFrame:Wpf", "xFrame.WPF.Controls.Windows")]
[assembly: XmlnsDefinition("xFrame:Wpf", "xFrame.WPF.Controls.Fluent")]
[assembly: XmlnsDefinition("xFrame:Wpf.WindowShell", "xFrame.WPF.Controls.WindowShell")]
