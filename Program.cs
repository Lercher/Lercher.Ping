using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;

namespace Lercher.Ping1
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
            var qy = from a in SinglePing.Tracert(addr) select a.ToString();
            var trace = string.Join("->", qy.ToArray());
            log(string.Format("*START* {0}", trace));
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
        private static int timeout5s = 5000;
        private static int timeout1s = 1000;
        private static Ping ping = new Ping();

        public static IEnumerable<IPAddress> Tracert(string addr)
        {
            for (var ttl = 1; ttl < 128; ttl++)
            {
                var po = new PingOptions(ttl, dontFragment: true);
                var data = "abcdefghijklmnopqrstuvwxyz012345";
                var buffer = System.Text.Encoding.ASCII.GetBytes(data);
                PingReply reply;
                try
                {
                    reply = ping.Send(addr, timeout1s, buffer, po);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException.Message);
                    yield break;
                }
                if (reply.Status == IPStatus.TtlExpired)
                {
                    yield return reply.Address;
                }
                else if (reply.Status == IPStatus.Success)
                {
                    yield return reply.Address;
                    yield break;
                }
                else
                {
                    yield break;
                }
            }
        }

        public static string Ping(string addr)
        {
            var ttl = 128;
            var po = new PingOptions(ttl, dontFragment: true);
            var data = "abcdefghijklmnopqrstuvwxyz012345";
            var buffer = System.Text.Encoding.ASCII.GetBytes(data);
            try
            {
                var reply = ping.Send(addr, timeout5s, buffer, po);
                if (reply.Status == IPStatus.Success)
                {
                    long next100msRoundtripTime = ((reply.RoundtripTime + 99) / 100) * 100;
                    if (buffer.SequenceEqual(reply.Buffer))
                    {
                        Console.Write("{0} OK - ", DateTime.Now);
                        Console.Write("Address: {0}, ", reply.Address);
                        Console.Write("RoundTrip time: {0}, ", reply.RoundtripTime);
                        Console.Write("Time to live: {0}, ", reply.Options.Ttl);
                        Console.Write("Don't fragment: {0}, ", reply.Options.DontFragment);
                        Console.Write("Buffer size: {0}", reply.Buffer.Length);
                        System.Console.WriteLine();
                        return string.Format("OK within {0}ms", next100msRoundtripTime);
                    }
                    else
                    {
                        return string.Format("Buffer mismatch within {0}ms", next100msRoundtripTime);
                    }
                }
                else
                {
                    var suffix = "";
                    var last = Tracert(addr).LastOrDefault();
                    if (last != null)
                        suffix = String.Format(", last good {1}", reply.Status, last);
                    Console.WriteLine("{0} FA - ping {1} failed: {2}{3}", DateTime.Now, addr, reply.Status, suffix);
                    return String.Format("{0}{1}", reply.Status, suffix);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return ex.InnerException.Message;
                return ex.Message;
            }
        }
    }
}
