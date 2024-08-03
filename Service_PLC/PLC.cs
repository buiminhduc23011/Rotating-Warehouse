using IndustrialNetworks.ModbusCore.ASCII;
using IndustrialNetworks.ModbusCore.Comm;
using IndustrialNetworks.ModbusCore.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_PLC
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
        public static void Connect(string COM)
        {
            try
            {
                objIModbusMaster = new ModbusASCIIMaster(COM, 9600, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.Even);
                objIModbusMaster.Connection();
                Isconnect = true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Isconnect = false;
                
            }
        }
        public static void Reconnect(string COM)
        {
            try
            {
                objIModbusMaster.Disconnection();
                objIModbusMaster = new ModbusASCIIMaster(COM, 9600, 7, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.Even);
                objIModbusMaster.Connection();
                Isconnect = true;
              
            }
            catch (Exception ex)
            {
                Isconnect = false;
                Console.WriteLine(ex.ToString());
            }
        }
        public static ushort ReadD(uint Address)
        {
            try
            {
                byte[] bytes = objIModbusMaster.ReadHoldingRegisters(1, Address, 1);

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
        public static void Pos(short Row, short Column)
        {
            try
            {
                short[] shorts = new short[4];
                shorts[0] = Row;//D501
                shorts[1] = Column;//D502
                shorts[2] = 1;//D503
                shorts[3] = 1;//D504
                byte[] values = Int.ToByteArray(shorts);
                objIModbusMaster.WriteMultipleRegisters(1, 4597, values);
            }
            catch (Exception ex)
            {
                Isconnect = false;
                Console.WriteLine(ex.Message);
            }
        }
        public static void Request_Run_To_PLC()
        {
            try
            {
                short[] shorts = new short[1];
                shorts[0] = 0;
                byte[] values = Int.ToByteArray(shorts);
                objIModbusMaster.WriteMultipleRegisters(1, 4599, values);//D503
            }
            catch (Exception ex)
            {
                Isconnect = false;
                Console.WriteLine(ex.Message);
            }
        }
        public static void Origin()
        {
            try
            {
                short[] shorts = new short[1];
                shorts[0] = 1;
                byte[] values = Int.ToByteArray(shorts);
                objIModbusMaster.WriteMultipleRegisters(1, 4596, values);// D
            }
            catch (Exception ex)
            {
                Isconnect = false;
                Console.WriteLine(ex.Message);
            }
        }
        public static void Done_product()
        {
            try
            {
                short[] shorts = new short[1];
                shorts[0] = 1;
                byte[] values = Int.ToByteArray(shorts);
                objIModbusMaster.WriteMultipleRegisters(1, 4600, values);// D
            }
            catch (Exception ex)
            {
                Isconnect = false;
                Console.WriteLine(ex.Message);
            }
        }
    }
}
