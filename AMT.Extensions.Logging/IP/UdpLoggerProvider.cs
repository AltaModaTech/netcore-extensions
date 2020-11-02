// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace AMT.Extensions.Logging.IP
{
    /// <summary>
    /// An <see cref="ILoggerProvider"/> the sends logs over UDP.
    /// </summary>
    [ProviderAlias("Udp")]
    public class UdpLoggerProvider : ILoggerProvider //, ISupportExternalScope
    {
        public ILogger CreateLogger(string category)
        {
            return new UdpLogger(this, _logger, category);
        }

        #region IDisposable impl

        /// <inheritdoc />
        public void Dispose()
        {
            _disposer?.Invoke();
        }

        #endregion IDisposable impl

        readonly Action _disposer;
        readonly ILogger _logger;
    }

    private class UdpLoggerProvider_OLD : ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, UdpLogger> _loggers = new ConcurrentDictionary<string, UdpLogger>();

        private readonly Func<string, LogLevel, bool> _filter;
        private IUdpLoggerSettings _settings;
        private readonly UdpLoggerProcessor _messageQueue = new UdpLoggerProcessor();

        private static readonly Func<string, LogLevel, bool> trueFilter = (cat, level) => true;
        private static readonly Func<string, LogLevel, bool> falseFilter = (cat, level) => false;
        private IDisposable _optionsReloadToken;
        private bool _includeScopes;
        private bool _disableColors;
        private IExternalScopeProvider _scopeProvider;
        private string _timestampFormat;
        private LogLevel _logToStandardErrorThreshold;

        public UdpLoggerProvider(Func<string, LogLevel, bool> filter, bool includeScopes)
            : this(filter, includeScopes, false)
        {
        }

        public UdpLoggerProvider(Func<string, LogLevel, bool> filter, bool includeScopes, bool disableColors)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _filter = filter;
            _includeScopes = includeScopes;
            _disableColors = disableColors;
        }

        public UdpLoggerProvider(IOptionsMonitor<UdpLoggerOptions> options)
        {
            // Filter would be applied on LoggerFactory level
            _filter = trueFilter;
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        private void ReloadLoggerOptions(UdpLoggerOptions options)
        {
            _includeScopes = options.IncludeScopes;
            _disableColors = options.DisableColors;
            _timestampFormat = options.TimestampFormat;
            _logToStandardErrorThreshold = options.LogToStandardErrorThreshold;
            var scopeProvider = GetScopeProvider();
            foreach (var logger in _loggers.Values)
            {
                logger.ScopeProvider = scopeProvider;
                logger.DisableColors = options.DisableColors;
                logger.TimestampFormat = options.TimestampFormat;
                logger.LogToStandardErrorThreshold = options.LogToStandardErrorThreshold;
            }
        }

        public UdpLoggerProvider(IUdpLoggerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;

            if (_settings.ChangeToken != null)
            {
                _settings.ChangeToken.RegisterChangeCallback(OnConfigurationReload, null);
            }
        }

        private void OnConfigurationReload(object state)
        {
            try
            {
                // The settings object needs to change here, because the old one is probably holding on
                // to an old change token.
                _settings = _settings.Reload();

                _includeScopes = _settings?.IncludeScopes ?? false;

                var scopeProvider = GetScopeProvider();
                foreach (var logger in _loggers.Values)
                {
                    logger.Filter = GetFilter(logger.Name, _settings);
                    logger.ScopeProvider = scopeProvider;
                }
            }
            catch (Exception ex)
            {
                System.Udp.WriteLine($"Error while loading configuration changes.{Environment.NewLine}{ex}");
            }
            finally
            {
                // The token will change each time it reloads, so we need to register again.
                if (_settings?.ChangeToken != null)
                {
                    _settings.ChangeToken.RegisterChangeCallback(OnConfigurationReload, null);
                }
            }
        }

        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name, CreateLoggerImplementation);
        }

        private UdpLogger CreateLoggerImplementation(string name)
        {
            var includeScopes = _settings?.IncludeScopes ?? _includeScopes;
            var disableColors = _settings is UdpLoggerSettings settings ? settings.DisableColors : _disableColors;

            return new UdpLogger(name, GetFilter(name, _settings), includeScopes? _scopeProvider: null, _messageQueue)
                {
                    DisableColors = disableColors,
                    TimestampFormat = _timestampFormat,
                    LogToStandardErrorThreshold = _logToStandardErrorThreshold
                };
        }

        private Func<string, LogLevel, bool> GetFilter(string name, IUdpLoggerSettings settings)
        {
            if (_filter != null)
            {
                return _filter;
            }

            if (settings != null)
            {
                foreach (var prefix in GetKeyPrefixes(name))
                {
                    LogLevel level;
                    if (settings.TryGetSwitch(prefix, out level))
                    {
                        return (n, l) => l >= level;
                    }
                }
            }

            return falseFilter;
        }

        private IEnumerable<string> GetKeyPrefixes(string name)
        {
            while (!string.IsNullOrEmpty(name))
            {
                yield return name;
                var lastIndexOfDot = name.LastIndexOf('.');
                if (lastIndexOfDot == -1)
                {
                    yield return "Default";
                    break;
                }
                name = name.Substring(0, lastIndexOfDot);
            }
        }

        private IExternalScopeProvider GetScopeProvider()
        {
            if (_includeScopes && _scopeProvider == null)
            {
                _scopeProvider = new LoggerExternalScopeProvider();
            }
            return _includeScopes ? _scopeProvider : null;
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _messageQueue.Dispose();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
}