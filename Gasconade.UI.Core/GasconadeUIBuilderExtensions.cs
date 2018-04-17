using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Gasconade.UI.Core
{
    /// <summary>
    /// Extension method for injecting the Gasconade UI middleware
    /// </summary>
    public static class GasconadeUIBuilderExtensions
    {
        /// <summary>
        /// Inject Gasconade UI middleware
        /// </summary>
        public static IApplicationBuilder UseGasconadeUI(this IApplicationBuilder app, Action<GasconadeUiConfigProxy> setupAction = null)
        {
            var callerAssm = Assembly.GetCallingAssembly();
            setupAction?.Invoke(new GasconadeUiConfigProxy());

            GasconadeUi.AddAssembly(callerAssm);

            app.UseMiddleware<GasconadeUiIndexMiddleware>();
            return app;
        }
    }
}