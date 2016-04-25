using System.IO;

namespace OrionLib.Utils
{
    public static class StreamExtensions
    {
        public static string ReadStringToEnd(this Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}