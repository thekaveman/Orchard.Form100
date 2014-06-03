using System;
using System.Linq;
using System.Collections.Generic;
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

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Wraps the Any() LINQ method in a null-check.
        /// </summary>
        public static bool SafeAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        /// <summary>
        /// Wraps the Any() LINQ method in a null-check.
        /// </summary>
        public static bool SafeAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable != null && enumerable.Any(predicate);
        }
    }
}