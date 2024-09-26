using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace network_util {
    
public static class SNI{
    
public static void ShowNetworkInterfaces()
{
    IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
    Console.WriteLine("Interface information for {0}.{1}     ",
            computerProperties.HostName, computerProperties.DomainName);
    if (nics == null || nics.Length < 1)
    {
        Console.WriteLine("  No network interfaces found.");
        return;
    }

    Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
    foreach (NetworkInterface adapter in nics)
    {
        IPInterfaceProperties properties = adapter.GetIPProperties();
        Console.WriteLine();
        Console.WriteLine(adapter.Description);
        Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length,'='));
        Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
        Console.WriteLine("  Physical Address ........................ : {0}",
                   adapter.GetPhysicalAddress().ToString());
        Console.WriteLine("  Operational status ...................... : {0}",
            adapter.OperationalStatus);
        string versions ="";

        // Create a display string for the supported IP versions.
        if (adapter.Supports(NetworkInterfaceComponent.IPv4))
        {
             versions = "IPv4";
         }
        if (adapter.Supports(NetworkInterfaceComponent.IPv6))
        {
            if (versions.Length > 0)
            {
                versions += " ";
             }
            versions += "IPv6";
        }
        Console.WriteLine("  IP version .............................. : {0}", versions);

        // The following information is not useful for loopback adapters.
        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
        {
            continue;
        }
        Console.WriteLine("  DNS suffix .............................. : {0}",
            properties.DnsSuffix);

        string label;
        if (adapter.Supports(NetworkInterfaceComponent.IPv4))
        {
            IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
            Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
            if (ipv4.UsesWins)
            {

                IPAddressCollection winsServers = properties.WinsServersAddresses;
                if (winsServers.Count > 0)
                {
                    label = "  WINS Servers ............................ :";
                }
            }
        }

        Console.WriteLine("  DNS enabled ............................. : {0}",
            properties.IsDnsEnabled);
        Console.WriteLine("  Receive Only ............................ : {0}",
            adapter.IsReceiveOnly);
        Console.WriteLine("  Multicast ............................... : {0}",
            adapter.SupportsMulticast);

        Console.WriteLine();
    }
    }
}
    public class Utils
    {

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            return address;
            //throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

    }
}