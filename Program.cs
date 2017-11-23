using System;

namespace Lercher.Ping
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is Lercher.Ping, (c) 2017 by Cassiopae Gmbh, M. Lercher");
            var addr = "192.168.1.34"; // 172.29.248.109 -> vmwbuild.kfra1.cassiopae.lan
            var ping = new System.Net.NetworkInformation.Ping();
            var po = new System.Net.NetworkInformation.PingOptions();
            var data = "abcdefghijklmnopqrstuvwxyz012345";
            var buffer = System.Text.Encoding.ASCII.GetBytes(data);
            var timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            var reply = ping.Send(addr, timeout, buffer, po); 
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine ("Address: {0}", reply.Address);
                Console.WriteLine ("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine ("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine ("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine ("Buffer size: {0}", reply.Buffer.Length);
            } else {
                System.Console.WriteLine("ping {0} failed: {1}", addr, reply.Status);
            }            
        }   
    }
}
