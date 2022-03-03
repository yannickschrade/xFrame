using System.Runtime.InteropServices;
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

[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM
// bereitgestellt wird.

[assembly: Guid("da91b385-7401-4281-98da-30bdf2a73f89")]


[assembly: XmlnsDefinition("xFrame:Wpf","xFrame.WPF")]
[assembly: XmlnsDefinition("xFrame:Wpf","xFrame.WPF.ViewInjection")]
