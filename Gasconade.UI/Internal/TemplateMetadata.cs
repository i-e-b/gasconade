using System.Collections.Generic;

namespace Gasconade.UI.Internal
{
    internal class TemplateMetadata
    {
        public string Template { get; set; }
        public LogTemplateDescription Description { get; set; }
        public Dictionary<string,string> Properties { get; set; }
        public bool IsCorrectHierarchy { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}