using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Rotating_Warehouse.Class
{
    public static class ini
    {
        public static string COM_PLC = "";
        public static void init(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                COM_PLC = (string)jsonObject["Port_PLC"];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }    
    }
}
