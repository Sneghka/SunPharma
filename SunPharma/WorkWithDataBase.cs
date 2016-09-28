using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace SunPharma
{
    public class WorkWithDataBase
    {
        
        public static void StoreDataFromDB(out RowDataList morionData, string query)
        {
            string connectionString = "Database=morion_2009:morion_database; User=TESTER; Password=654654";
            FbConnection conn = new FbConnection(connectionString);
            conn.Open();
            
            if (conn.State == ConnectionState.Closed) conn.Open();

            FbTransaction myTransaction = conn.BeginTransaction();
            FbCommand myCommand = new FbCommand(query, conn);
            myCommand.Transaction = myTransaction;
            FbDataReader reader = myCommand.ExecuteReader();
            var rowDataList = new RowDataList();
            try
            {
                while (reader.Read())
                {
                    var monthPreformat = reader["PERIOD_ID"].ToString().Trim().Replace(" ","").Substring(4,2);
                    var yearPreformat = reader["PERIOD_ID"].ToString().Trim().Replace(" ", "").Substring(0,4);
                    var rowData = new RowData
                    {
                        Distributor = reader["Name_BASIC_RUS"].ToString().Trim(),
                        SalesMonthPcs = Math.Round(Convert.ToDecimal(reader["Q_RECALC"]), 1),
                        Month = Convert.ToInt32(monthPreformat),
                        Year = Convert.ToInt32(yearPreformat)
                    };
                    rowDataList.Add(rowData);
                }
            }
            finally
            {
                conn.Close();
            }

            myCommand.Dispose();
            morionData = rowDataList;
        }
    }
}
