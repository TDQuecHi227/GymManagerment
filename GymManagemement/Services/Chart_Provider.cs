using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;
using System.Drawing;

namespace GymManagemement.Services
{
    public class Chart_Provider
    {
        ConnDB db = new ConnDB();
        public List<(string Label, int Value)> GetMonthlyRevenueData()
        {
            var result = new List<(string, int)>();
            string sql = @"
    SELECT 
        MONTH(transaction_date) AS Month, 
        SUM(total_amount) AS Total 
    FROM transactions 
    WHERE YEAR(transaction_date) = YEAR(GETDATE())
    GROUP BY MONTH(transaction_date) 
    ORDER BY Month";
            DataSet ds = db.ExecuteQueryData(sql, CommandType.Text);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int month = Convert.ToInt32(row["Month"]);
                int total = Convert.ToInt32(row["Total"]);
                result.Add(($"Tháng {month}", total));
            }
            return result;
        }

        public List<(string Label, int Value)> GetYearlyRevenueData()
        {
            var result = new List<(string, int)>();
            string sql = "SELECT YEAR(transaction_date) AS Year, SUM(total_amount) AS Total FROM transactions GROUP BY YEAR(transaction_date) ORDER BY Year";
            DataSet ds = db.ExecuteQueryData(sql, CommandType.Text);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string year = row["Year"].ToString();
                int total = Convert.ToInt32(row["Total"]);
                result.Add((year, total));
            }
            return result;
        }

        public List<(string Label, int Value)> GetDailyRevenueData()
        {
            var vietnameseDays = new List<string>
{
    "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"
};

            // ánh xạ từ tiếng Anh → tiếng Việt
            var dayMap = new Dictionary<string, string>
            {
                ["Monday"] = "Thứ hai",
                ["Tuesday"] = "Thứ ba",
                ["Wednesday"] = "Thứ tư",
                ["Thursday"] = "Thứ năm",
                ["Friday"] = "Thứ sáu",
                ["Saturday"] = "Thứ bảy",
            };

            var tempResult = new Dictionary<string, int>();

            string sql = @"SELECT
                            DATENAME(weekday, transaction_date) AS DayName,
                            SUM(total_amount) AS Total
                        FROM transactions
                        WHERE transaction_date >= DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
  AND transaction_date <  DATEADD(DAY, 7 - DATEPART(WEEKDAY, GETDATE()) + 1, CAST(GETDATE() AS DATE))
  AND transaction_date <= GETDATE()
                        GROUP BY DATENAME(weekday, transaction_date)";
            DataSet ds = db.ExecuteQueryData(sql, CommandType.Text);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string engDay = row["DayName"].ToString();
                if (dayMap.TryGetValue(engDay, out string vietDay))
                {
                    int total = Convert.ToInt32(row["Total"]);
                    tempResult[vietDay] = total;
                }
            }

            // Sắp xếp đúng thứ tự VN
            var result = vietnameseDays
                .Select(day => (Label: day, Value: tempResult.ContainsKey(day) ? tempResult[day] : 0))
                .ToList();

            return result;
        }

    }
}
