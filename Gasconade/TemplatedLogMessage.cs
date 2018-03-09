using System;
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
            var attrs = GetType().GetCustomAttributes(typeof(LogMessageTemplateAttribute), true) as LogMessageTemplateAttribute[];
            if (attrs == null || attrs.Length < 1) return UntemplatedMessage();

            var tmpl = attrs.First().MessageTemplate.Replace("{", "{0:"); // we subvert the `string.Format` logic a bit here...

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
            
            var attrs = GetType().GetCustomAttributes(typeof(LogMessageDescriptionAttribute), true) as LogMessageDescriptionAttribute[];
            if (attrs == null || attrs.Length < 1) return new LogTemplateDescription();

            var desc = attrs.First();

            return new LogTemplateDescription{
                Causes = desc.Causes,
                Actions = desc.Actions,
                Description = desc.Description
            };
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