using CSM.Form100.Models;
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
}