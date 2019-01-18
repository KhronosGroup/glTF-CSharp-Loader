using System.IO;

namespace glTFLoader
{
    internal static class StreamExtensions
    {
        public static void Align(this Stream stream, int size, byte fillByte = 0)
        {
            var mod = stream.Position % size;
            if (mod == 0)
            {
                return;
            }

            for (var i = 0; i < size - mod; i++)
            {
                stream.WriteByte(fillByte);
            }
        }
 
    
#if NET35
        // Only useful before .NET 4
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
#endif
    }
}
