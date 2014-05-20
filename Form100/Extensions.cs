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

    public static class QueueExtensions
    {
        public static Queue<T> Copy<T>(this Queue<T> source)
        {
            Queue<T> copy = new Queue<T>(source);

            return copy;
        }
    }
}