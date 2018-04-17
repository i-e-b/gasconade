using System.Reflection;

namespace Gasconade.UI
{
    /// <summary>
    /// A configuration helper. Calls down to the static GasconadeUi class
    /// </summary>
    public class GasconadeConfiguratorProxy
    {
        /// <summary>
        /// Add another assembly to be scanned for log messages.
        /// This can be used like `.AddAssembly(typeof(MyMessageClass).Assembly)`
        /// </summary>
        public void AddAssembly(Assembly assembly) => GasconadeUi.AddAssembly(assembly);

        /// <summary>
        /// Include a link back to Swagger (or another URL) in Gasconade
        /// </summary>
        public void AddSwaggerLink(string link = null) => GasconadeUi.AddSwaggerLink(link);

        /// <summary>
        /// Additional HTML added to the top of the Gasconade UI page
        /// </summary>
        public void AddHeaderHtml(string html) => GasconadeUi.HeaderHtml = html;
    }
}