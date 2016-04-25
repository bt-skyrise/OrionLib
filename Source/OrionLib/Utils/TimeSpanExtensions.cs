using System;
using System.Xml;

namespace OrionLib.Utils
{
    public static class TimeSpanExtensions
    {
        public static string ToIso8601(this TimeSpan timeSpan)
        {
            return XmlConvert.ToString(timeSpan);
        }
    }
}