using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using IndustrialNetworks.ModbusCore.ASCII;
using IndustrialNetworks.ModbusCore.Comm;
using IndustrialNetworks.ModbusCore.DataTypes;
using Rotating_Warehouse.Class;

namespace Rotating_Warehouse
{
    
    public static class PLC
    {
        public static bool Isconnect;
        private static IModbusMaster objIModbusMaster = null;
        public static void Disconnect()
        {
            objIModbusMaster.Disconnection();
            Isconnect = false;
        }
        public static void Connect()
        {
            try
            {
                objIModbusMaster = new ModbusASCIIMaster(ini.COM_PLC, 9600, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.Even);
                objIModbusMaster.Connection();
                Isconnect = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Isconnect = false;
            }
        }
        public static void Reconnect()
        {
            try
            {
                objIModbusMaster.Disconnection();
                objIModbusMaster = new ModbusASCIIMaster(ini.COM_PLC, 9600, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.Even);
                objIModbusMaster.Connection();
                Isconnect = true;
            }
            catch (Exception ex)
            {
                Isconnect = false;
            }
        }
        public static int ReadD(uint Address)
        {
            try
            {
                byte[] bytes = objIModbusMaster.ReadHoldingRegisters(1,Address, 1);

                if (bytes != null)
                {
                    ushort[] result = Word.ToArray(bytes);
                    return result[0];
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }
        public static void Write_R_C(short R, short C)
        {
            try
            {
                short[] shorts = new short[4];
                shorts[0] = R;
                shorts[1] = C;
                shorts[2] = 0;
                shorts[3] = 1;
                byte[] values = Int.ToByteArray(shorts);
                objIModbusMaster.WriteMultipleRegisters(1, 4597, values);
            }
            catch (Exception ex)
            {
                Isconnect = false;
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
