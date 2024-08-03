using System;
using System.Collections.Generic;
using IndustrialNetworks.ModbusCore.ASCII;
using IndustrialNetworks.ModbusCore.Comm;
using IndustrialNetworks.ModbusCore.DataTypes;
using System.IO.Ports;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SocketIOClient;
using System.Net.NetworkInformation;
using System.CodeDom.Compiler;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Text.Json;

namespace ConsolePLC
{
    public class Program
    {
        private IModbusMaster objIModbusMaster = null;
        public bool Connected = false;
        SocketIO socketclient;
        Data data = new Data();
        private bool Sendmac = false;
        private bool running = true;
        private static string host_socket;
        private static string COM;
        private static bool isInitialized = false;
        //
        public static ushort Done_Origin;
        public static bool flag_done_Origin = false;
        public static ushort Done_Pos;
        public static bool flag_done_Pos = false;
        //
        static void Initialize()
        {
            if (!isInitialized)
            {
                string json = File.ReadAllText("scnn.ini");
                var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                host_socket = data_Setting["Host_Socket"];
                COM = data_Setting["Comport"];
                isInitialized = true;
            }
        }
        static async Task Main(string[] args)
        {
            Program program = new Program();
            Initialize();
            Console.WriteLine("MAC Address: " + GetMacAddress());
            PLC.Connect(COM);

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                program.running = false;
                PLC.Disconnect();
            };

            Task plcTask = Task.Run(() => program.RunPLC());
            Task reconnectTask = Task.Run(() => program.RunReconnect());
            Task listenServerTask = Task.Run(() => program.RunListenServer());

            await Task.WhenAll(plcTask, reconnectTask, listenServerTask);
            PLC.Disconnect();
        }
        public void Close()
        {
            objIModbusMaster.Disconnection();
        }
        public void RunReconnect()
        {
            
            while (running && PLC.Isconnect== false)
            {
                PLC.Reconnect(COM);
                Thread.Sleep(1000);
            }
        }

        public void RunPLC()
        {
            while (running && PLC.Isconnect)
            {
                Done_Origin = PLC.ReadD(4616);
                Done_Pos = PLC.ReadD(4617);
                // Console.WriteLine(Done_Pos.ToString());
                Console.WriteLine("Vị Trí Hiện Tại là: "+ PLC.ReadD(4619));
                Console.WriteLine("Vị Trí Mục Tiêu là: "+ PLC.ReadD(4620));
                Thread.Sleep(100);
            }
        }
        public async void RunListenServer()
        {
            while (running)
            {
                try
                {
                    if (!Connected)
                    {
                        socketclient = new SocketIO(host_socket);
                        await socketclient.ConnectAsync();
                        Connected = true;
                    }
                    if (Sendmac == false && Connected == true)
                    {
                        var mac = new
                        {
                            mac = GetMacAddress(),
                        };
                        await socketclient.EmitAsync("connected-server", mac);
                        Sendmac = true;
                        Console.WriteLine("Server Connected: " + GetMacAddress());
                    }
                    socketclient.OnConnected += (eventSender, eventArgs) =>
                    {
                        Connected = true;
                    };

                    socketclient.OnDisconnected += (eventSender, eventArgs) =>
                    {
                        Connected = false;
                        Sendmac = false;
                    };
                    if(flag_done_Origin == false && Done_Origin ==1)
                    {
                        var data = new
                        {
                            mac = GetMacAddress(),
                        };
                        await socketclient.EmitAsync("success-original-detector", data);
                        flag_done_Origin = true;
                        Console.WriteLine("success-original-ditector");
                    }
                    if (flag_done_Pos == false && Done_Pos == 1)
                    {
                        var data = new
                        {
                            mac = GetMacAddress(),
                        };
                        await socketclient.EmitAsync("rb-success-call-location", data);
                        flag_done_Pos = true;
                        Console.WriteLine("rb-success-call-location");
                    }

                    //
                    //    if(pre_value_error!= data.Error_machine_1)
                    //    {
                    //    var mes = new
                    //    {2349
                    //        mac = GetMacAddress(),
                    //        error = data.Error_machine_1,
                    //    };
                    //    await socketclient.EmitAsync("robot-error", mes);
                    //    pre_value_error = data.Error_machine_1;
                    //    }
                    //    if (data.Status_Org == 0) flag_done_org = false;
                    //    if(flag_done_org==false && data.Status_Org!=0 )
                    //    {
                    //    var mes = new
                    //    {
                    //        mac = GetMacAddress(),
                    //    };
                    //    await socketclient.EmitAsync("success-original-detector", mes);
                    //    flag_done_org = true;
                    //}
                    //    //
                    //if (data.Status_Goods == 0) flag_done_pos = false;
                    //if (flag_done_pos == false && data.Status_Goods != 0)
                    //{
                    //    var mes = new
                    //    {
                    //        mac = GetMacAddress(),
                    //    };
                    //    await socketclient.EmitAsync("rb-success-call-location", mes);
                    //    flag_done_pos = true;
                    //}
                    //
                    socketclient.On("call-origin-detector", response =>
                    {
                        Console.WriteLine("call-origin-detector");
                        PLC.Origin();
                        Thread.Sleep(2000);
                        flag_done_Origin = false;
                    });
                        socketclient.On("call-robot", response =>
                    {
                        string res = response.ToString();
                       
                        List<ObjectPos> myObjects = JsonConvert.DeserializeObject<List<ObjectPos>>(res);
                        if (myObjects.Count > 0)
                        {
                            ObjectPos firstObject = myObjects[0];
                            short rowValue = firstObject.Row;
                            short columnValue = firstObject.Column;
                            Console.WriteLine("Pos:" + $"row: {rowValue}, column: {columnValue}");
                            PLC.Pos(rowValue, columnValue);
                            //Console.WriteLine("Đã ghi vị trí xuống PLC");
                            //Console.Write("Waitting");
                            //for (int i = 0; i<1000;i++)
                            //{
                            //    Console.Write(".");
                            //    Thread.Sleep(200);
                            //}
                            //Console.WriteLine("");
                            //Thread.Sleep(2000);
                            //PLC.Request_Run_To_PLC();
                            //Console.WriteLine("Yêu Cầu Kho Chạy Đến Vị Trí Đã Ghi");
                            flag_done_Pos = false;
                        }
                        else
                        {
                            Console.WriteLine("Mảng JSON không chứa đối tượng.");
                        }
                    });

                    socketclient.On("sv-success-call-location", response =>
                    {
                        Console.WriteLine("sv-success-call-location");
                        PLC.Done_product();
                    });
                }
                catch
                {
                    Console.WriteLine("Error Conncet: " + host_socket);
                }
                Thread.Sleep(1); 
            }
        }
        public static string GetMacAddress()
        {
            string macAddress = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up && !nic.Description.ToLower().Contains("virtual") && !nic.Description.ToLower().Contains("pseudo"))
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) // Check if it's a Wi-Fi interface
                    {
                        byte[] macBytes = nic.GetPhysicalAddress().GetAddressBytes();
                        macAddress = string.Join(":", macBytes.Select(b => b.ToString("X2")));
                        break;
                    }

                }
            }
            return macAddress;
        }

    }
}
