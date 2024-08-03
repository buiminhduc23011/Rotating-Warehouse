using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotating_Warehouse
{
    public class Handel_Data
    {
        public static string GetShifts()
        {
            string Shifts = "";
            TimeSpan C1 = TimeSpan.Parse("06:00:00");
            TimeSpan C2 = TimeSpan.Parse("13:51:00");
            TimeSpan C3 = TimeSpan.Parse("21:45:00");
            TimeSpan Noww = DateTime.Now.TimeOfDay;
            if (Noww >= C1 && Noww < C2)
            {
                Shifts = "Ca 1";
            }
            else if (Noww >= C2 && Noww < C3)
            {
                Shifts = "Ca 2";
            }
            else if (Noww >= C3)
            {
                Shifts = "Ca 3";
            }
            else
            {
                Shifts = "Ca 3";
            }
            return Shifts;
        }
    }
}
