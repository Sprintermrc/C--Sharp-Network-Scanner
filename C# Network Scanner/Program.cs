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
            "\n6 - Проверить активные IP адреса в указанном диапазоне" +
            "\n7 - Показать список адресов в указанном диапазоне с конкретным открытым портом" +
            "\n Ваш выбор: ");

            int a = Int32.Parse(Console.ReadLine());
            if ( a == 1 ) // 1 вариант Показать все активные соединения
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
                Console.Read();
                return;
            }

            else if ( a == 2 )  //2 вариант Показать список всех активных сетевых интерфейсов
            {    

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
                Console.Read();
                return;
            }
            else if ( a == 3 )          //3 вариант Показать список всех сетевых интерфейсов (2 вариант)
            {
                //  ipScanner.act();
                SNI.ShowNetworkInterfaces();
                Console.Read();
                return;                
            }
            else if ( a == 4 )          //4 вариант Показать список открытых портов на {string_ip}
            {
                if (PingClass.BoolPing(string_ip, 1, 1000) == false)
                {
                    Console.WriteLine("Error: host is down");
                    Console.Read();
                    return;
                }
                Console.WriteLine("Введите диапазон сканирования портов (1-n (максимум 65535) или * чтобы сканировать все порты):");
                Console.WriteLine("Введите первое значение"); 
                string scan_range = Console.ReadLine();
                if (scan_range.Equals("*"))
                {
                    PortScannerClass.PortScanCustom(string_ip, 1, 65535); 
                }
                else
                {
                    int scan_range_min = Int32.Parse(scan_range);
                    Console.WriteLine("Введите максимальное значение");
                    int scan_range_max = Int32.Parse(Console.ReadLine());
                    PortScannerClass.PortScanCustom(string_ip, scan_range_min, scan_range_max);
                }
                Console.Read();
                return;                
            }
            else if ( a == 5)           //5 вариант Показать список открытых портов на выбранном IP
            {
              
                Console.WriteLine("Введите необходимый IP адресс и диапазон сканирования портов (1-n (максимум 65535) или * чтобы сканировать все порты):");
                Console.WriteLine("Введите IP для сканирования: "); 
                string scan_string_ip = Console.ReadLine();
                if (PingClass.BoolPing(scan_string_ip, 1, 1000) == false)
                {
                    Console.WriteLine("Error: host is down");
                    Console.Read();
                    return;
                }                  
                Console.WriteLine("Введите первое значение: "); 
                string scan_range = Console.ReadLine();
                if (scan_range.Equals("*"))
                {
                    PortScannerClass.PortScanCustom(scan_string_ip, 1, 65535);
                }
                else
                {
                    int scan_range_min = Int32.Parse(scan_range);
                    Console.WriteLine("Введите максимальное значение");
                    int scan_range_max = Int32.Parse(Console.ReadLine());
                    PortScannerClass.PortScanCustom(scan_string_ip, scan_range_min, scan_range_max);     
                }
                Console.Read();
                return;                           
            }
            else if( a == 6 )           //6 вариант  Проверить активные IP адреса в указанном диапазоне
            {
                Console.WriteLine("Введите диапазон IP для сканирования ");
                Console.WriteLine("Введите начало диапазона IP для сканирования: "); 
                string scan_string_ip_start = Console.ReadLine();
                Console.WriteLine("Введите конец диапазона IP для сканирования: "); 
                string scan_string_ip_end =  Console.ReadLine();        
                // string scan_string_ip_start = "172.29.9.1";
                // string scan_string_ip_end = "172.29.9.6";     
                AllPing.scan2(scan_string_ip_start, scan_string_ip_end);
            }
            else if ( a == 7 )          //7 вариант Показать список адресов в указанном диапазоне с конкретным открытым портом
            {
                Console.WriteLine("Введите диапазон IP для сканирования ");
                Console.WriteLine("Введите начало диапазона IP для сканирования: "); 
                string scan_string_ip_start = Console.ReadLine();
                Console.WriteLine("Введите конец диапазона IP для сканирования: "); 
                string scan_string_ip_end =  Console.ReadLine();  
                Console.WriteLine("Введите первое значение диапазона портов: "); 
                string scan_range = Console.ReadLine();
                if (scan_range.Equals("*"))
                {
                    AllPing.scan2_spec_ports(scan_string_ip_start, scan_string_ip_end, "1", "65535");
                }   
                else
                {
                    int scan_range_min = Int32.Parse(scan_range);
                    Console.WriteLine("Введите максимальное значение диапазона портов:");
                    string scan_range_max = Console.ReadLine();

                    AllPing.scan2_spec_ports(scan_string_ip_start, scan_string_ip_end, scan_range_min.ToString(), scan_range_max);                    
                }
                Console.Read();
                return;
            }
        }
    }
}