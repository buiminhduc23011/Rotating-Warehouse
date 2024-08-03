using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_PLC
{
    public class ObjectPos
    {
        [JsonProperty("row")]
        public short Row { get; set; }

        [JsonProperty("column")]
        public short Column { get; set; }
    }
}
