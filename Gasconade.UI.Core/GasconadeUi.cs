using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gasconade.UI.Core
{
    /// <summary>
    /// Options and settings for Gasconade UI middleware
    /// </summary>
    public class GasconadeUi
    {
        /// <summary>
        /// Path prefix that the UI will be available on
        /// </summary>
        public static string RoutePrefix { get; set; }

        /// <summary>
        /// Additional HTML added to the top of the Gasconade UI page
        /// </summary>
        public static string HeaderHtml { get; set; }

        private static readonly List<Type> _discoveredTypes = new List<Type>();
        internal static string StylesheetText;
        internal static string ScriptText;
        internal static string ReturnLink;

        /// <summary>
        /// Create new settings with defaults
        /// </summary>
        static GasconadeUi()
        {
            RoutePrefix = "gasconade";
        }

        /// <summary>
        /// Provide raw stylesheet data to be included in the page
        /// </summary>
        public static void SetStylesheet(string stylesheet) {
            StylesheetText = stylesheet;
        }

        /// <summary>
        /// Provide raw javascript to be included in the page
        /// </summary>
        public static void SetJavascript(string script) {
            ScriptText = script;
        }

        /// <summary>
        /// Build a link to the gasconade docs, with given text.
        /// This is designed to work from Swagger UI
        /// </summary>
        public static string Link(string text)
        {
            return "<a href=\"../../gasconade\">"+text+"</a>";
        }

        /// <summary>
        /// Include a link back to Swagger in Gasconade
        /// </summary>
        public static void AddSwaggerLink(string link = null)
        {
            ReturnLink = link ?? "../../swagger";
        }

        /// <summary>
        /// Remove all discovered log message types
        /// </summary>
        public static void ClearTypes()
        {
            _discoveredTypes.Clear();
        }

        /// <summary>
        /// Add another assembly to be scanned for log messages
        /// </summary>
        public static void AddAssembly(Assembly assm) {
            _discoveredTypes.AddRange(GetTemplateTypes(assm));
        }

        /// <summary>
        /// Return all log message types discovered so far
        /// </summary>
        public static IEnumerable<Type> KnownLogTypes() {
            return _discoveredTypes.Select(id=>id);
        }
        
        private static List<Type> GetTemplateTypes(Assembly assm)
        {
            return assm.GetTypes().Where(IsLogTemplate).ToList();
        }
        
        private static bool IsLogTemplate(Type arg)
        {
            return arg.CustomAttributes.Any(a => a.AttributeType == typeof(LogMessageTemplateAttribute));
        }
    }
}
