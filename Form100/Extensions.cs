using System;
using System.Xml.Linq;

namespace CSM.Form100
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Helper method to safely get an attribute value.
        /// </summary>
        /// <param name="node">An XElement node to read an attribute value from</param>
        /// <param name="attributeName">The attribute to read</param>
        /// <returns>The attribute's value if it exists, or the empty string</returns>
        public static string SafeGetAttribute(this XElement node, string attributeName)
        {
            try
            {
                return node.Attribute(attributeName).Value;
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}