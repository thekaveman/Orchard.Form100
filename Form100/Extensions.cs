using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CSM.Form100
{
    public static class XElementExtensions
    {
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

    public static class CollectionExtensions
    {
        public static Queue<T> Copy<T>(this Queue<T> source)
        {
            Queue<T> copy = new Queue<T>(source);

            return copy;
        }

        public static Stack<T> Copy<T>(this Stack<T> source)
        {
            Stack<T> copy = new Stack<T>(source);

            return copy;
        }
    }
}