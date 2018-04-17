using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tag;

namespace Gasconade.UiCore
{
    /// <summary>
    /// Class for creating page content
    /// </summary>
    public static class PageContentGenerator
    {
        /// <summary>
        /// A page noting that no log messages were found
        /// </summary>
        public static TagContent EmptyLogsPage()
        {
            return 
                T.g("body")[
                    T.g("h1")["No logs found"],
                    T.g("p")["This site either does not have any logs, or Gasconade has not been correctly configured"],
                    T.g("p")["Check you have called 'GasconadeUi.AddAssembly(typeof(AMessageType).Assembly);' "]
                ] ;
        }

        /// <summary>
        /// Create a html body that lists out all the known log message types with descriptions
        /// </summary>
        public static TagContent ListingDocument(List<Type> all, string returnLink = null, TagContent heading = null)
        {
            var lines = T.g();
            lines.Add(T.g("i")["Gasconade "]);
            if (! string.IsNullOrWhiteSpace(returnLink)) {
                lines.Add(T.g("a", "href",returnLink)["Return to API docs"]);
            }
            lines.Add(T.g("h1")["Log Documentation"]);

            if (heading != null) {
                lines.Add(heading);
            }

            var sorted = all.Select(ReadTemplateMetadata).OrderBy(m=>m.Description.IsObsolete).ThenBy(m=>m.Name);

            foreach (var info in sorted)
            {
                var block = T.g("div", "class","MessageBlock");

                AddTitle(info, block);
                AddTemplate(info, block);
                block.Add(BuildDetailsExpander(info));

                lines.Add(block);
            }

            return lines;
        }

        private static void AddTemplate(TemplateMetadata info, TagContent block)
        {
            if (string.IsNullOrWhiteSpace(info.Template))
            {
                block.Add(T.g("i")["This message has no template, and will be given a default message"]);
            }
            else if (info.Description.IsObsolete)
            {
                block.Add(T.g("code", "class", "obsoleteTemplate")[info.Template]);
            }
            else
            {
                block.Add(T.g("code", "class", "template")[info.Template]);
            }
        }

        private static void AddTitle(TemplateMetadata info, TagContent block)
        {
            if (!string.IsNullOrWhiteSpace(info.Description.RetirementMessage))
            {
                block.Add(T.g("p", "class", "titleNote")[info.Description.RetirementMessage]);
            }

            block.Add(T.g("h3", "class", "header", "id", info.FullName)[info.Name]);
            if (!info.IsCorrectHierarchy)
            {
                AddSubtypeWarning(block);
            }
        }

        private static TemplateMetadata ReadTemplateMetadata(Type type)
        {
            return new TemplateMetadata{
                Name = type.Name,
                FullName = type.FullName,
                IsCorrectHierarchy = type.GetTypeInfo().IsSubclassOf(typeof(TemplatedLogMessage)),
                Description = TemplatedLogMessage.GetDescription(type),
                Properties = TemplatedLogMessage.GetPropertyDescriptions(type),
                Template = TemplatedLogMessage.GetTemplateText(type)
            };
        }

        private static TagContent BuildDetailsExpander(TemplateMetadata info)
        {
            var expando = T.g("div",  "class","expando",  "id","expand_" + info.FullName);
            if (!string.IsNullOrWhiteSpace(info.Description.Description))
            {
                expando.Add(T.g("h4")["Description"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Description]);
            }

            if (!string.IsNullOrWhiteSpace(info.Description.Causes))
            {
                expando.Add(T.g("h4")["Causes"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Causes]);
            }

            if (!string.IsNullOrWhiteSpace(info.Description.Actions))
            {
                expando.Add(T.g("h4")["Actions"]);
                expando.Add(T.g("p", "class", "informative")[info.Description.Actions]);
            }

            if (info.Properties.Count > 0)
            {
                expando.Add(T.g("h4")["Properties"]);
                var defs = T.g("dl");
                foreach (var prop in info.Properties)
                {
                    defs.Add(T.g("dt")[prop.Key]);
                    defs.Add(T.g("dd")[prop.Value]);
                }

                expando.Add(defs);
            }

            return expando;
        }

        private static void AddSubtypeWarning(TagContent block)
        {
            block.Add(T.g("p")[
                T.g("b")["Note: "],
                "This type does not derive from ",
                T.g("code")["Gasconade.TemplatedLogMessage"],
                " and can't be sent"
            ]);
        }
    }
}