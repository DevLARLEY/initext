using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace initext
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("initext by github.com/DevLARLEY");
            if (args.Length >= 1)
            {
                if (File.Exists(args[0]))
                {
                    byte[] data = File.ReadAllBytes(args[0]);
                    string hex = Convert.ToHexString(data);
                    IEnumerable<int> pssh = hex.indexes("70737368");
                    ArrayList res = new ArrayList();
                    foreach (var r in pssh)
                    {
                        int s = Convert.ToInt32(hex.Substring(r - 8, 8), 16) * 2;
                        if (s < 100)
                        {
                            continue;
                        }
                        string f = hex.Substring(r + 56, s - 56);
                        int p = 0;
                        while (true)
                        {
                            if (p > f.Length)
                            {
                                break;
                            }
                            string h = f.Substring(p, 2);
                            if (h.Equals("48"))
                            {
                                p += 12;
                            }
                            else
                            {
                                int s2 = Convert.ToInt32(f.Substring(p + 2, 2), 16) * 2;
                                if (h.Equals("12"))
                                {
                                    res.Add(new string[] { f.Substring(p + 4, s2), hex.Substring(r - 8, s), Convert.ToBase64String(Convert.FromHexString(hex.Substring(r - 8, s))) });
                                }
                                p += s2 + 2;
                            }
                        }
                    }
                    if (res.Count == 0)
                    {
                        Console.WriteLine("No pssh found that contains a key id.");
                    }
                    else
                    {
                        foreach (string[] r in res)
                        {
                            Console.WriteLine("=> Key ID: " + r[0]);
                            Console.WriteLine("   Base16 PSSH: " + r[1]);
                            Console.WriteLine("   Base64 PSSH: " + r[2]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("input file does not exist");
                }
            }
            else
            {
                Console.WriteLine("  Usage: initext <init.mp4 path>");
            }
        }

        static IEnumerable<int> indexes(this string str, string search)
        {
            int minIndex = str.IndexOf(search);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(search, minIndex + search.Length);
            }
        }
    }
}
