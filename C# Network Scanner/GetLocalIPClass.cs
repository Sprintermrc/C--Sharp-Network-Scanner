using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace network_util
{
    public static class getLocalIPClass
    {
        public static string GetLocalIPAddress()
        {   
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }

public static class PingClass
{

public static bool BoolPing (string host, int attempts, int timeout)
   {
        //Stopwatch swBoolPing = new Stopwatch();
       // swBoolPing.Start();
      System.Net.NetworkInformation.Ping  ping = new System.Net.NetworkInformation.Ping ();

      System.Net.NetworkInformation.PingReply  pingReply;

      for (int i = 0; i < attempts; i++)
      {
         try
         {
            pingReply = ping.Send (host, timeout); 

            // If there is a successful ping then return true.
            if (pingReply != null &&
                pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
               return true;
         }
         catch
         {
            // Do nothing and let it try again until the attempts are exausted.
            // Exceptions are thrown for normal ping failurs like address lookup
            // failed.  For this reason we are supressing errors.
         }
      }

      // Return false if we can't successfully ping the server after several attempts.
      //swBoolPing.Stop();
     // Console.WriteLine("BoolPing elapsed: " + swBoolPing.ElapsedMilliseconds);
      return false;
   }
}
}
