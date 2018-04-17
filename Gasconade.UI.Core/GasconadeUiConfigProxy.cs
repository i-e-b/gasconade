using System.Reflection;

namespace Gasconade.UI.Core
{
    /// <summary>
    /// Configuration helper. Calls down to static config.
    /// </summary>
    public class GasconadeUiConfigProxy
    {
        /// <summary>
        /// Provide raw stylesheet data to be included in the page
        /// </summary>
        public void SetStylesheet(string stylesheet) => GasconadeUi.SetStylesheet(stylesheet);

        /// <summary>
        /// Provide raw javascript to be included in the page
        /// </summary>
        public void SetJavascript(string script) => GasconadeUi.SetJavascript(script);

        /// <summary>
        /// Include a link back to Swagger in Gasconade
        /// </summary>
        public void AddSwaggerLink(string link = null) => GasconadeUi.AddSwaggerLink(link);

        /// <summary>
        /// Remove all discovered log message types
        /// </summary>
        public void ClearTypes() => GasconadeUi.ClearTypes();

        /// <summary>
        /// Add another assembly to be scanned for log messages
        /// </summary>
        public void AddAssembly(Assembly assm) => GasconadeUi.AddAssembly(assm);
        
        /// <summary>
        /// Additional HTML added to the top of the Gasconade UI page
        /// </summary>
        public void AddHeaderHtml(string html) => GasconadeUi.HeaderHtml = html;
    }
}