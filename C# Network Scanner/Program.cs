using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace network_util
{
    class Program
    {
        static void Main(string[] args)
        {
            string string_ip = getLocalIPClass.GetLocalIPAddress();
            var ip = IPAddress.Parse(string_ip);
            List<String> ipAdresses = new List<String>(); 

            Console.WriteLine("Local IP Adress: " + string_ip);
            Console.WriteLine("");
            Console.WriteLine("Выберите опцию:");
            Console.WriteLine("1 - Показать все активные соединения" +
            "\n2 - Показать список всех активных сетевых интерфейсов " +
            "\n3 - Показать список всех сетевых интерфейсов (2 вариант)" +
            $"\n4 - Показать список открытых портов на {string_ip}" +
            "\n5 - Показать список открытых портов на выбранном IP" +
            "\n Ваш выбор: ");

            int a = Int32.Parse(Console.ReadLine());
            if ( a == 1 ) // 1 вариант
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

                foreach (TcpConnectionInformation t in connections)
                {
                    Console.Write("Local endpoint: {0} ", t.LocalEndPoint);
                    Console.Write("Remote endpoint: {0} ", t.RemoteEndPoint);
                    Console.WriteLine("State: {0}", t.State);
                    //Console.WriteLine($"Ping success : {PingClass.BoolPing(t.RemoteEndPoint.ToString(), 1, 1)}");
                }
                Console.WriteLine("");
                Console.ReadKey();
                return;
            }

            else if ( a == 2 )
            {    //2 вариант

                Console.WriteLine("Список сетевых интерфейсов:");
                foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    Console.WriteLine("Name: " + netInterface.Name);
                    Console.WriteLine("Description: " + netInterface.Description);
                    Console.WriteLine("Addresses: ");

                    IPInterfaceProperties ipProps = netInterface.GetIPProperties();

                    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                    {
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            String StringAddr = addr.Address.ToString();
                            Console.WriteLine(" " + addr.Address.ToString());
                            ipAdresses.Add(StringAddr);
                            Console.WriteLine($"Is responding to ping: {PingClass.BoolPing(StringAddr, 2, 2)} ");
                            Console.WriteLine("Маска подсети: " + Utils.GetSubnetMask(addr.Address));
                        }
                    }
                    Console.WriteLine("");              
                }

                Console.WriteLine("----------------------------------");
                Console.WriteLine("Список всех IPv4 адресов: ");
                for (int i = 0; i < ipAdresses.Count; i++)
                {
                    Console.WriteLine(ipAdresses[i]);
                    //ipScanner.act(ipAdresses[i], "255.255.255.0");
                }

                Console.WriteLine("--------------------------------");
                Console.ReadKey();
                return;
            }
            else if ( a == 3 )
            {
                //  ipScanner.act();
                SNI.ShowNetworkInterfaces();
                Console.ReadKey();
                return;                
            }
            else if ( a == 4 )
            {
                Console.WriteLine("Введите диапазон сканирования портов (1-n (максимум 65535) или * чтобы сканировать все порты):");
                Console.WriteLine("Введите первое значение"); 
                string scan_range = Console.ReadLine();
                if (scan_range.Equals("*"))
                {
                    PortScannerClass.PortScanCutom(string_ip, 1, 65535); 
                }
                else
                {
                    int scan_range_min = Int32.Parse(scan_range);
                    Console.WriteLine("Введите максимальное значение");
                    int scan_range_max = Int32.Parse(Console.ReadLine());
                    PortScannerClass.PortScanCutom(string_ip, scan_range_min, scan_range_max);
                }
                Console.ReadKey();
                return;                
            }
            else if ( a == 5)
            {
                Console.WriteLine("Введите необходимый IP адресс и диапазон сканирования портов (1-n (максимум 65535) или * чтобы сканировать все порты):");
                Console.WriteLine("Введите IP для сканирования: "); 
                string scan_string_ip = Console.ReadLine();
                Console.WriteLine("Введите первое значение: "); 
                string scan_range = Console.ReadLine();
                if (scan_range.Equals("*"))
                {
                    PortScannerClass.PortScanCutom(scan_string_ip, 1, 65535);
                }
                else
                {
                    int scan_range_min = Int32.Parse(scan_range);
                    Console.WriteLine("Введите максимальное значение");
                    int scan_range_max = Int32.Parse(Console.ReadLine());
                    PortScannerClass.PortScanCutom(scan_string_ip, scan_range_min, scan_range_max);     
                }
                Console.ReadKey();
                return;                           
            }
        }
    }
}