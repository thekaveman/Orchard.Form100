using System;
using System.Xml.Linq;

namespace CSM.Form100.Drivers
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
}