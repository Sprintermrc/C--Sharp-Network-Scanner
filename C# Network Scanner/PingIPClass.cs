using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace network_util
{
    public static class AllPing
    {

        public static async void scan2(string start, string end)
        {
            Stopwatch swPingIp = new Stopwatch();
            swPingIp.Start();
            try 
            {
                //Split IP string into a 4 part array
                string[] startIPString = start.Split('.');
                int[] startIP = Array.ConvertAll<string, int>(startIPString, int.Parse); //Change string array to int array
                string[] endIPString = end.Split('.');
                int[] endIP = Array.ConvertAll<string, int>(endIPString, int.Parse);
                int count = 0; //Count the number of successful pings
                Ping myPing;
                //PingReply reply;
                IPAddress addr;
                IPHostEntry host;
                var tcs = new TaskCompletionSource<int>();
                //Loops through the IP range, maxing out at 255
                for (int i = startIP[2]; i <= endIP[2]; i++) { //3rd octet loop
                    for (int y = startIP[3]; y <= 255; y++) { //4th octet loop
                        string ipAddress = startIP[0] + "." + startIP[1] + "." + i + "." + y; //Convert IP array back into a string
                        string endIPAddress = endIP[0] + "." + endIP[1] + "." + endIP[2] + "." + (endIP[3] + 1); // +1 is so that the scanning stops at the correct range
                        // Console.WriteLine("---------------" + ipAddress.ToString() + " --------------------");
                        //If current IP matches final IP in range, break
                        if (ipAddress == endIPAddress) 
                        {
                            Console.WriteLine("fin");
                                swPingIp.Stop();
                                TimeSpan ts2 = swPingIp.Elapsed;
                                Console.WriteLine("Ping IPs elapsed: " + ts2);
                            break;
                        }

                        myPing = new Ping();
                        try 
                        {
                         //     reply = myPing.Send(ipAddress, 500); //Ping IP address with 500ms timeout
                            if (PingClass.BoolPing(ipAddress.ToString(), 1, 10) == true)
                            {
                                //PortScannerClass.RunCustomPortScanAsync(ipAddress, 0, 1000);
                                
                                Console.WriteLine(ipAddress.ToString() + " is UP");
                                try 
                                {
                                    addr = IPAddress.Parse(ipAddress);
                                    host = Dns.GetHostEntry(addr);
                                    Console.WriteLine(ipAddress.ToString() + " " + host.HostName.ToString() + " is Up");
                                    
                                    //  listVAddr.Items.Add(new ListViewItem(new String[] { ipAddress, host.HostName, "Up" })); //Log successful pings
                                    count++;
                                }
                                catch 
                                {
                                 //   Console.WriteLine(ipAddress.ToString() + " Could not retrieve, " + "Up");
                                //Logs pings that are successful, but are most likely not windows machines
                                    count++;
                                }
                            }
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine("timeout");
                            break;
                        }
                    }
                    startIP[3] = 1; //If 4th octet reaches 255, reset back to 1
                    await tcs.Task;
                    
                }    
            }
            catch 
            {

            }

        }



       public static async void scan2_spec_ports(string start, string end, string scan_port_start, string scan_port_end)
        {
                    Stopwatch swPingIP_and_ScanSpecPorts = new Stopwatch();
                    swPingIP_and_ScanSpecPorts.Start(); 
            try 
            {

                //Split IP string into a 4 part array
                string[] startIPString = start.Split('.');
                int[] startIP = Array.ConvertAll<string, int>(startIPString, int.Parse); //Change string array to int array
                string[] endIPString = end.Split('.');
                int[] endIP = Array.ConvertAll<string, int>(endIPString, int.Parse);
                int count = 0; //Count the number of successful pings
                //Ping myPing;
                // PingReply reply;
                IPAddress addr;
                IPHostEntry host;
                var tcs = new TaskCompletionSource<int>();
                //Loops through the IP range, maxing out at 255

                for (int i = startIP[2]; i <= endIP[2]; i++) 
                { //3rd octet loop
                    for (int y = startIP[3]; y <= 255; y++) 
                    { //4th octet loop
                        string ipAddress = startIP[0] + "." + startIP[1] + "." + i + "." + y; //Convert IP array back into a string
                        string endIPAddress = endIP[0] + "." + endIP[1] + "." + endIP[2] + "." + (endIP[3] + 1); // +1 is so that the scanning stops at the correct range
                        // Console.WriteLine("---------------" + ipAddress.ToString() + " --------------------");
                        //If current IP matches final IP in range, break
                        if (ipAddress == endIPAddress) 
                        {
                            Console.WriteLine("fin");
                            break;
                        }
                      
                            // using Ping ping = new();
                            // PingReply reply = await ping.SendPingAsync(ipAddress, 10);
                       // myPing = new Ping();
                         if (PingClass.BoolPing(ipAddress.ToString(), 1, 10) == true)
                         {
                            //reply = myPing.Send(ipAddress, 10);

                            try 
                            {

                                //PortScannerClass.RunCustomPortScanAsync(ipAddress, 0, 1000);
                                
                                PortScannerClass.PortScanCustom(ipAddress, Int32.Parse(scan_port_start), Int32.Parse(scan_port_end));

                                try 
                                {
                                    addr = IPAddress.Parse(ipAddress);
                                    host = Dns.GetHostEntry(addr);
                                    Console.WriteLine(ipAddress.ToString() + " " + host.HostName.ToString() + " is Up");
                                    
                                    //  listVAddr.Items.Add(new ListViewItem(new String[] { ipAddress, host.HostName, "Up" })); //Log successful pings
                                    count++;
                                }
                                catch 
                                {
                                //   Console.WriteLine(ipAddress.ToString() + " Could not retrieve, " + "Up");
                                //Logs pings that are successful, but are most likely not windows machines
                                    count++;
                                }
                            
                            }
                        
                            catch (Exception ex) 
                            {
                                Console.WriteLine("timeout");
                                break;
                            }

                        }

                    }
                   
                    startIP[3] = 1; //If 4th octet reaches 255, reset back to 1
                    await tcs.Task;
                    
                }    
            }
            catch 
            {

            }
                swPingIP_and_ScanSpecPorts.Stop();
                TimeSpan ts1 = swPingIP_and_ScanSpecPorts.Elapsed;
                Console.WriteLine("Ping ip and scan spec ports elsaped " + ts1.ToString()); 
        }        

    }
}