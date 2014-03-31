using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemsStatus.Common.Helpers
{
    public static class UrlHelper
    {
        public static string GenerateFriendlyUrl(string name)
        {
            // make it all lower case
            name = name.ToLower();
            // remove entities
            name = Regex.Replace(name, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            name = Regex.Replace(name, @"[^a-z0-9\-\s]", "");
            // replace spaces
            name = name.Replace(' ', '-');
            // collapse dashes
            name = Regex.Replace(name, @"-{2,}", "-");
            // trim excessive dashes at the beginning
            name = name.TrimStart(new[] { '-' });
            // if it's too long, clip it
            if (name.Length > 80)
                name = name.Substring(0, 79);
            // remove trailing dashes
            name = name.TrimEnd(new[] { '-' });
            return name;
        }
    }
}
