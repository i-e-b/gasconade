using System;
using System.Collections.Generic;
using System.Linq;

namespace Gasconade
{
    /// <summary>
    /// Base class for templated log messages
    /// </summary>
    public abstract class TemplatedLogMessage {
        /// <summary>
        /// Expand template with properties from derived instance
        /// </summary>
        public override string ToString()
        {
            var tmpl = GetTemplateText(GetType());
            if (string.IsNullOrWhiteSpace(tmpl)) return UntemplatedMessage();

            tmpl = tmpl
                .Replace("{", "{0:")        // we subvert the `string.Format` logic a bit here to work with `PropertyFormatProvider`
                .Replace("{0:{0:", "{{");   // restore escaped text

            return string.Format(new PropertyFormatProvider(), tmpl, this);
        }

        /// <summary>
        /// If a template is not supplied, generate a default template
        /// </summary>
        private string UntemplatedMessage()
        {
            var type = GetType();
            var typeName = type.Name;
            var props = type.GetProperties().Where(p=>p.CanRead).Select(p=>p.Name + " = '" + p.GetValue(this) + "'");

            return "Message of type: " + typeName + "; " + string.Join(", ", props);
        }

        /// <summary>
        /// Extract the description data from the derived instance
        /// </summary>
        public LogTemplateDescription GetDescription() {
            return GetDescription(GetType());
        }

        /// <summary>
        /// Get a dictionary of (prop name)=>(descriptive text) for all properties public in the type
        /// </summary>
        public static Dictionary<string, string> GetPropertyDescriptions(Type target)
        {
            var props = target.GetProperties().Where(p=>p.CanRead);//.Select(p=>p.Name + " = '" + p.GetValue(this) + "'");

            var outp = new Dictionary<string,string>();

            foreach (var p in props)
            {
                var attrs = p.GetCustomAttributes(typeof(LogParamAttribute), true) as LogParamAttribute[];
                var desc = "";
                if (attrs != null && attrs.Length > 0) desc = attrs.First().Description;
                outp.Add(p.Name, desc);
            }

            return outp;
        }

        /// <summary>
        /// Extract the description data from a type definition
        /// </summary>
        public static LogTemplateDescription GetDescription(Type target) {
            if (target == null) return null;

            var descAttrs = target.GetCustomAttributes(typeof(LogMessageDescriptionAttribute), true) as LogMessageDescriptionAttribute[];
            if (descAttrs == null || descAttrs.Length < 1) return new LogTemplateDescription();
            var desc = descAttrs.Single();

            return new LogTemplateDescription
            {
                Causes = desc.Causes,
                Actions = desc.Actions,
                Description = desc.Description
            };
        }
        
        /// <summary>
        /// Get the raw message template for the log message type
        /// </summary>
        public static string GetTemplateText(Type target) {
            var attrs = target.GetCustomAttributes(typeof(LogMessageTemplateAttribute), true) as LogMessageTemplateAttribute[];
            if (attrs == null || attrs.Length < 1) return null;

            return attrs.Single().MessageTemplate;
        }

        /// <summary>
        /// This format provider abuses the 'format' string to look up property values on the parameter object
        /// </summary>
        private class PropertyFormatProvider : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                try
                {
                    var prop = arg.GetType().GetProperty(format);
                    if (prop == null) return "{?" + format + "}"; // no such property (template is wrong?)
                    return prop.GetValue(arg)?.ToString() ?? "<null>";
                }
                catch
                {
                    return "<error>";
                }
            }
        }
    }
}