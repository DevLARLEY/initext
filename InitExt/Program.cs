using System.Diagnostics;

namespace InitExt
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var executableName = Process.GetCurrentProcess().MainModule?.FileName;
            Console.WriteLine($"""
            initext by github.com/DevLARLEY
            Usage: {Path.GetFileName(executableName)} <init file>
            """);

            if (args.Length == 0)
            {
                Console.WriteLine("No input file specified.");
                return;
            }
            
            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"The input file '{args[0]}' does not exist.");
                return;
            }

            var widevineSystemId = new byte[] { 0xed, 0xef, 0x8b, 0xa9, 0x79, 0xd6, 0x4a, 0xce, 0xa3, 0xc8, 0x27, 0xdc, 0xd5, 0x1d, 0x21, 0xed };
            
            var bytes = File.ReadAllBytes(args[0]);
            var indices = FindIndices(bytes, "pssh"u8.ToArray());
            
            foreach (var index in indices)
            {
                var size = GetSize(bytes, index-4);
                var data = bytes[(index - 4)..(index - 4 + size)];
                
                if (!data[12..28].SequenceEqual(widevineSystemId))
                    continue;
                
                Console.WriteLine(Convert.ToBase64String(data));
            }
        }

        private static int GetSize(byte[] bytes, int index)
        {
            var sizeBytes = bytes[index..(index + 4)];
            Array.Reverse(sizeBytes);
            return BitConverter.ToInt32(sizeBytes, 0);
        }
        
        private static IEnumerable<int> FindIndices(byte[] source, byte[] pattern)
        {
            for (var i = 0; i < source.Length; i++)
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    yield return i;
        }
    }
}