using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunPharma
{
    public class RowData
    {
        public string Distributor { get; set; }
        public decimal SalesMonthPcs { get; set; }
        public decimal PlanMonthPcs { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string DistibutorFormattedName
        {
            get
            {
                return Distributor.IndexOf(".") == -1 ? Distributor : Distributor.Substring(Distributor.IndexOf('.') + 2);
            }
        }

        public bool IsEqual(RowData anotherOne)
        {
            if (Distributor == anotherOne.Distributor && SalesMonthPcs == anotherOne.SalesMonthPcs &&
                Year == anotherOne.Year && Month == anotherOne.Month)
           
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        


    }
}
