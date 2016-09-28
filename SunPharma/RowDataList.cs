using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunPharma
{
    public class RowDataList: List<RowData>
    {

        public static List<string> CompareStrings(List<string> list1, List<string> list2)
        {
            //return list1.Where(s => !list2.Contains(s)).ToList();
            return (from s in list1
                    where !list2.Contains(s)
                    select s).ToList();
        }

        public static List<RowData> CompareRowDataObjects(RowDataList list1, RowDataList list2)
        {
            return (from l1 in list1
                    join l2 in list2 on new { Distibutor = l1.Distributor, l1.SalesMonthPcs, l1.Year, l1.Month } equals new  { Distibutor = l2.Distributor, l2.SalesMonthPcs, l2.Year, l2.Month } into r
                    from g in r.DefaultIfEmpty()
                    where g == null
                    select l1).ToList();
        }

        public static List<RowData> CompareRowDataObjects2(RowDataList list1, RowDataList list2)
        {

            return (from l1 in list1
                    where !list2.Any(l1.IsEqual)
                    select l1).ToList();

            //list1.Where (r => !list2.Any (t => t.IsEqual( r ) ) )
        }
    }
}
