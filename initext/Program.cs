using System.Collections;
using System.Text.RegularExpressions;

namespace initext
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("initext by github.com/DevLARLEY\n");
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
                        string sysid = hex.Substring(r + 16, 32);
                        if (sysid.Equals("EDEF8BA979D64ACEA3C827DCD51D21ED"))
                        {
                            int s = Convert.ToInt32(hex.Substring(r - 8, 8), 16) * 2;
                            if (s < 100)
                            {
                                continue;
                            }
                            string f = hex.Substring(r + 56, s - 64);
                            ArrayList t = new ArrayList();
                            t.Add(hex.Substring(r - 8, s));
                            int p = 0;
                            while (p < f.Length)
                            {
                                string h = f.Substring(p, 2);
                                switch (h)
                                {
                                    case "08":
                                        p += 4;
                                        break;
                                    case "48":
                                        p += 12;
                                        break;
                                    default:
                                        int s2 = Convert.ToInt32(f.Substring(p + 2, 2), 16) * 2;
                                        if (h.Equals("12"))
                                        {
                                            t.Add(f.Substring(p + 4, s2));
                                        }
                                        p += s2 + 4;
                                        break;
                                }
                            }
                            res.Add(t.ToArray());
                        }
                        else if (sysid.Equals("9A04F07998404286AB92E65BE0885F95"))
                        {
                            int s = Convert.ToInt32(hex.Substring(r - 8, 8), 16) * 2;
                            if (s < 90)
                            {
                                continue;
                            }
                            int os = Convert.ToInt32(swap_endian(hex.Substring(r + 56, 8)), 16) * 2;
                            string obj = hex.Substring(r + 64, os-8);
                            ArrayList t = new ArrayList();
                            t.Add(hex.Substring(r - 8, s));
                            int p = 4;
                            for (int i = 1; i <= Convert.ToInt32(swap_endian(obj.Substring(0, 4)), 16); i++)
                            {
                                int s2 = Convert.ToInt32(swap_endian(obj.Substring(p + 4, 4)), 16) * 2;
                                int type = Convert.ToInt32(swap_endian(obj.Substring(p, 4)), 16);
                                if (type == 1)
                                {
                                    Regex rg = new Regex("<KID ALGID=\".{6}\" VALUE=\".{24}\">");
                                    MatchCollection matches = rg.Matches(hex_to_ascii(obj.Substring(p + 8, s2)));
                                    foreach (var match in matches)
                                    {
                                        t.Add(BitConverter.ToString(Convert.FromBase64String(match.ToString().Substring(27, 24))).Replace("-", "").ToLower());
                                    }
                                }
                                p += s2+8;
                            }
                            res.Add(t.ToArray());
                        }
                    }

                    if (res.Count == 0)
                    {
                        Console.WriteLine("No pssh found that contains a key id.");
                    }
                    else
                    {
                        foreach (object[] o in res)
                        {
                            if (o.Length >= 2)
                            {
                                Console.WriteLine("PSSH (Base64): " + Convert.ToBase64String(Convert.FromHexString((string)o[0])));
                                for (int i = 1; i < o.Length; i++)
                                {
                                    Console.WriteLine("  |=> Key ID: " + o[i].ToString().ToLower());
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Input file does not exist.");
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

        static string swap_endian(string s)
        {
            if (s.Length % 2 != 0)
                s += "0";
            string[] a = Enumerable.Range(0, s.Length).GroupBy(x => x / 2).Select(x => new string(x.Select(y => s[y]).ToArray())).ToArray();
            Array.Reverse(a);
            return string.Join("", a);
        }

        static string hex_to_ascii(string hex)
        {
            hex = hex.Replace("00", "");
            string r = "";
            for (int i = 0; i < hex.Length/2; i++)
            {
                r += (char)Convert.ToInt32(hex.Substring(i * 2, 2), 16);
            }
            return r;
        }
    }
}
