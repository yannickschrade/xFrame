using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace xFrame.WPF.Hosting
{
    internal class ConfigurationProviderSource : IConfigurationSource
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ConfigurationProviderSource(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new IgnoreFirstLoadConfigurationProvider(_configurationProvider);
        }

        private sealed class IgnoreFirstLoadConfigurationProvider : IConfigurationProvider, IEnumerable<IConfigurationProvider>, IDisposable
        {
            private readonly IConfigurationProvider _provider;

            private bool _hasIgnoredFirstLoad;

            public IgnoreFirstLoadConfigurationProvider(IConfigurationProvider provider)
            {
                _provider = provider;
            }

            public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
            {
                return _provider.GetChildKeys(earlierKeys, parentPath);
            }

            public IChangeToken GetReloadToken()
            {
                return _provider.GetReloadToken();
            }

            public void Load()
            {
                if (!_hasIgnoredFirstLoad)
                {
                    _hasIgnoredFirstLoad = true;
                    return;
                }

                _provider.Load();
            }

            public void Set(string key, string value)
            {
                _provider.Set(key, value);
            }

            public bool TryGet(string key, out string value)
            {
                return _provider.TryGet(key, out value);
            }

            public IEnumerator<IConfigurationProvider> GetEnumerator() => GetUnwrappedEnumerable().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetUnwrappedEnumerable().GetEnumerator();

            public override bool Equals(object? obj)
            {
                return _provider.Equals(obj);
            }

            public override int GetHashCode()
            {
                return _provider.GetHashCode();
            }

            public override string? ToString()
            {
                return _provider.ToString();
            }

            public void Dispose()
            {
                (_provider as IDisposable)?.Dispose();
            }

            private IEnumerable<IConfigurationProvider> GetUnwrappedEnumerable()
            {
                yield return _provider;
            }
        }
    }
}
