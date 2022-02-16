using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;

namespace xFrame.WPF.Theming
{
    [ContentProperty(nameof(Resources))]
    public class ThemeExtension : DependencyObject
    {
        protected Theme extendedTheme;
        private string _name;
        public string ThemeName
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                ChangeExtendedTheme(value);
                _name = value;

            }
        }
        private ObservableCollection<ResourceDictionary> _resources;
        public Collection<ResourceDictionary> Resources
        {
            get
            {
                if (_resources == null)
                {
                    _resources = new ObservableCollection<ResourceDictionary>();
                    _resources.CollectionChanged += OnResourcesChanged;
                }
                return _resources;
            }
        }

        private void OnResourcesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ResourceDictionary resource in e.NewItems)
            {
                extendedTheme.MergedDictionaries.Add(resource);
            }
        }

        protected Uri _source;
        public virtual Uri Source
        {
            get => _source;
            set
            {
                if (_source == value)
                    return;
                CreateDictionary(value);
                _source = value;
            }
        }

        private void CreateDictionary(Uri value)
        {
            if(Resources.Count != 0)
                throw new NotSupportedException("use key or direct content");
            extendedTheme.MergedDictionaries.Add(new ResourceDictionary { Source = value });
        }

        private void ChangeExtendedTheme(string theme)
        {
            extendedTheme = ThemeManager.Current[theme];
        }
    }
}
