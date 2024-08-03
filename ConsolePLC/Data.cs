using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePLC
{
    public class Data
    {
        public ushort Status_Org { get; set; }
        public ushort Status_Goods { get; set; }
        public ushort Error_machine_1 { get; set; }
        //
        public ushort Call_Org { get; set; }
    }
}
