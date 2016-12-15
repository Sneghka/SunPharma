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

      
        public static void CompareRowDataObjects2(RowDataList list1, RowDataList list2)
        {
       var diff = (from l1 in list1
                    where !list2.Any(l1.IsEqual)
                    select l1).ToList();

            int i = 0;
            int j = 0;

            while( i <diff.Count && j < list2.Count )
            {
                if (diff[i].Distributor != list2[j].Distributor)
                    {
                        j++;
                        continue;
                    }

                    var output = diff[i].SalesMonthPcs - list2[j].SalesMonthPcs;
                    Console.WriteLine("Разница: список_1 " + diff[i].Distributor + " " + "month - " + diff[i].Month + " " + diff[i].SalesMonthPcs + " ----- " + list2[j].SalesMonthPcs + ". Разница - " + output);
                    i++;
                
            }
            //list1.Where (r => !list2.Any (t => t.IsEqual( r ) ) )
        }

        public static List<RowData> ComparePcsByDistributorByDate(RowDataList list1, RowDataList list2)
        {
            return (from l1 in list1
                    from l2 in
                        list2.Where(r => r.Year == l1.Year && r.Month == l1.Month && r.Distributor == l1.Distributor).DefaultIfEmpty()
                    select new RowData
                    {
                        Distributor = l1.Distributor,
                        Month = l1.Month,
                        Year = l1.Year,
                        SalesMonthPcs = l1.SalesMonthPcs - (l2?.SalesMonthPcs ?? 0)
                    }
                ).ToList();
        }
    


        /*  public decimal GetSalesPcsByDistributorDate(RowDataList list, string distributor, int month)
        {
             decimal pcs = from l in list
                where l.Distributor == distributor && l.Month == month
                select l.SalesMonthPcs;
        }*/

    }
}
