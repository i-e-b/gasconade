using System;
using System.Reflection;
using System.Web.Http;

namespace Gasconade.UI
{
    /// <summary>
    /// Configuration helper extension for HttpConfiguration object
    /// </summary>
    public static class GasconadeConfigExtensions {

        /// <summary>
        /// Add Gasconade UI responders to this configuration, and register log messages from the calling assembly
        /// </summary>
        public static void EnableGasconadeUi( this HttpConfiguration config, Action<GasconadeConfiguratorProxy> configCall) {
            var callerAssm = Assembly.GetCallingAssembly();

            GasconadeUi.Register(config);
            GasconadeUi.AddAssembly(callerAssm);

            configCall(new GasconadeConfiguratorProxy());
        }

        /// <summary>
        /// Add Gasconade UI responders to this configuration, and register log messages from the calling assembly
        /// </summary>
        public static void EnableGasconadeUi( this HttpConfiguration config) {
            var callerAssm = Assembly.GetCallingAssembly();

            GasconadeUi.Register(config);
            GasconadeUi.AddAssembly(callerAssm);
        }
    }
}