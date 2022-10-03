using System.Runtime.CompilerServices;
using System.Text;

namespace Task1
{
    // Необходимо заменить на более подходящий тип (коллекцию), позволяющий
    // эффективно искать диапазон по заданному IP-адресу
    using IPRangesDatabase = Object;

    public class Task1
    {
        /*
        * Объекты этого класса создаются из строки, но хранят внутри помимо строки
        * ещё и целочисленное значение соответствующего адреса. Например, для адреса
         * 127.0.0.1 должно храниться число 1 + 0 * 2^8 + 0 * 2^16 + 127 * 2^24 = 2130706433.
        */
        internal record IPv4Addr(string StrValue) : IComparable<IPv4Addr>
        {
            internal uint IntValue = Ipstr2Int(StrValue);

            private static uint Ipstr2Int(string strValue)
            {
                var a = strValue.Split('.');
                return strValue.Length == 0 ? 0 : (uint)(int.Parse(a[0]) * Math.Pow(2, 24) + int.Parse(a[1]) * Math.Pow(2, 16) +
                              int.Parse(a[2]) * Math.Pow(2, 8) + int.Parse(a[3]));
            }

            // Благодаря этому методу мы можем сравнивать два значения IPv4Addr
            public int CompareTo(IPv4Addr other)
            {
                return IntValue.CompareTo(other.IntValue);
            }

            public override string ToString()
            {
                return StrValue;
            }
        }

        internal record IPRange(IPv4Addr IpFrom, IPv4Addr IpTo)
        {
            public override string ToString()
            {
                return $"{IpFrom},{IpTo}";
            }
        }

        internal record IPLookupArgs(string IpsFile, List<string> IprsFiles);

        internal static IPLookupArgs? ParseArgs(string[] args)
        {
            if (args == Array.Empty<string>())
            {
                return null;
            }

            if (args.Length <= 1)
            {
                return null;
            }

            List<string> iprsFiles = new List<string> { args[1] };
            for (int i = 2; i < args.Length; i++)
            {
                iprsFiles.Add(args[i]);
            }

            return new IPLookupArgs(args[0], iprsFiles);
        }

        internal static List<string> LoadQuery(string filename)
        {
            return new List<string>(filename.Split('/').ToList());
        }

        internal record IPRangesDatabase(List<IPRange> IpRanges);

        internal static IPRangesDatabase? LoadRanges(List<String> filenames)
        {
            if (filenames.Count == 0)
            {
                return null;
            }
            
            List<IPRange> ipRanges = new List<IPRange>();
            foreach (var filename in filenames)
            {
                if (File.Exists(filename))
                {
                    StreamReader reader = File.OpenText(filename);
                    var result = reader.ReadToEnd();

                    string[] strings = result.Split('\n', '\r');
                    foreach (var thatString in strings)
                    {
                        if (thatString.Length != 0) ipRanges.Add(new IPRange(new IPv4Addr(thatString.Split(' ')[0]), new IPv4Addr(thatString.Split(' ')[1])));
                    }
                }
            }
            IPRangesDatabase ipRangesDatabase = new IPRangesDatabase(ipRanges);
            return ipRangesDatabase;
        }

        internal static string FindRange(IPRangesDatabase ranges, IPv4Addr query)
        {
            if (ranges == null)
            {
                return query.StrValue + " NO";
            }
            for (int i = 0; i < ranges.IpRanges.Count; i++)
            {
                if (query.CompareTo(ranges.IpRanges[i].IpFrom) >= 0 && query.CompareTo(ranges.IpRanges[i].IpTo) <= 0)
                {
                    return query.StrValue + $" YES({ranges.IpRanges[i].IpFrom} {ranges.IpRanges[i].IpTo})";
                }
            }

            return query.StrValue + " NO";
        }

        public static void Main(string[]? args)
        {
            var ipLookupArgs = ParseArgs(args);
            if (ipLookupArgs == null)
            {
                return;
            }

            var queries = LoadQuery(ipLookupArgs.IpsFile);
            var ranges = LoadRanges(ipLookupArgs.IprsFiles);
            foreach (var ip in queries)
            {
                var findRange = FindRange(ranges, new IPv4Addr(ip));
                Console.WriteLine($"{ip}: {findRange}");
            }
        }
        
    }
}