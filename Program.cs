using System;

namespace Lercher.Ping
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is Lercher.Ping, (c) 2017 by Cassiopae Gmbh, M. Lercher");
            if (args.Length == 1)
            {
                var addr = args[0]; // 172.29.248.109 -> vmwbuild.kfra1.cassiopae.lan
                var state = new State(addr);
                while (true)
                {
                    var r = SinglePing.Ping(addr);
                    state.log(r);
                    System.Threading.Thread.Sleep(10000);
                }
            }
            else
            {
                System.Console.WriteLine("usage: Lercher.ping <name-or-ipaddr>");
                System.Console.WriteLine("  pings every 10s with 32bytes and logs to ping.log");
            }
        }
    }

    public class State
    {
        public readonly string addr;
        public string state;
        public DateTime date = DateTime.Now;
        private long pos = 0L;
        private string fn = "ping.log";

        public State(string addr)
        {
            this.addr = addr;
            var fi = new System.IO.FileInfo(fn);
            if (fi.Exists)
                pos = fi.Length;
            log("*START*");
        }

        public void log(string state)
        {
            date = DateTime.Now;
            if (state == this.state)
            {
                using (var st = System.IO.File.OpenWrite(fn))
                using (var tw = new System.IO.StreamWriter(st, System.Text.Encoding.ASCII))
                {
                    tw.BaseStream.Position = pos;
                    tw.WriteLine("{0:s} {1} *LAST*", date, addr);
                }
                return;
            }
            this.state = state;
            var s = string.Format("{0:s} {1} {2}", date, addr, state);
            System.Console.WriteLine(s);
            using (var st = System.IO.File.OpenWrite(fn))
            using (var tw = new System.IO.StreamWriter(st, System.Text.Encoding.ASCII))
            {
                tw.BaseStream.Position = pos;
                tw.WriteLine(s);
                tw.Flush();
                pos = tw.BaseStream.Position;
                tw.BaseStream.SetLength(pos);
            }
        }
    }
    public static class SinglePing
    {
        public static string Ping(string addr)
        {
            var ping = new System.Net.NetworkInformation.Ping();
            var po = new System.Net.NetworkInformation.PingOptions();
            var data = "abcdefghijklmnopqrstuvwxyz012345";
            var buffer = System.Text.Encoding.ASCII.GetBytes(data);
            var timeout = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            var reply = ping.Send(addr, timeout, buffer, po);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.Write("{0} OK - ", DateTime.Now);
                Console.Write("Address: {0}, ", reply.Address);
                Console.Write("RoundTrip time: {0}, ", reply.RoundtripTime);
                Console.Write("Time to live: {0}, ", reply.Options.Ttl);
                Console.Write("Don't fragment: {0}, ", reply.Options.DontFragment);
                Console.Write("Buffer size: {0}", reply.Buffer.Length);
                System.Console.WriteLine();
                return string.Format("OK in {0}ms", ((reply.RoundtripTime + 99) / 100) * 100);
            }
            else
            {
                Console.WriteLine("{0} FA - ping {1} failed: {2}", DateTime.Now, addr, reply.Status);
                return reply.Status.ToString();
            }
        }
    }
}
