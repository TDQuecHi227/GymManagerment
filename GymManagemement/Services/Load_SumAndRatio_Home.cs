using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Connection;

namespace GymManagemement.Services
{
    public class Load_SumAndRatio_Home
    {
        ConnDB conn = new ConnDB();
        public int GetTotalMembers()
        {
            string query = "SELECT COUNT(*) FROM members";
            return conn.ExecuteScalar(query);
        }
        public int GetTotalTrainers()
        {
            string query = "SELECT COUNT(*) FROM trainers";
            return conn.ExecuteScalar(query);
        }
        public int GetTotalRevenue()
        {
            string query = "SELECT SUM(total_amount) FROM transactions";
            return conn.ExecuteScalar(query);
        }
        public string GetRatioMembers()
        {
            string query = "WITH MonthlyMembers AS (" +
                "SELECT " +
                "   FORMAT(join_date, 'yyyy-MM') AS Month," +
                "   COUNT(*) AS MemberCount " +
                "FROM members " +
                "GROUP BY FORMAT(join_date, 'yyyy-MM')), " +
                "TopTwoMonths AS (" +
                "SELECT TOP 2 *" +
                "FROM MonthlyMembers " +
                "ORDER BY Month DESC) " +
                "SELECT " +
                "   MAX(CASE WHEN rn = 1 THEN Month END) AS CurrentMonth, " +
                "   MAX(CASE WHEN rn = 1 THEN MemberCount END) AS CurrentCount, " +
                "   MAX(CASE WHEN rn = 2 THEN MemberCount END) AS PrevCount, " +
                "   CASE " +
                "       WHEN MAX(CASE WHEN rn = 2 THEN MemberCount END) IS NULL THEN NULL " +
                "       WHEN MAX(CASE WHEN rn = 2 THEN MemberCount END) = 0 THEN NULL " +
                "       ELSE ROUND(" +
                "               (MAX(CASE WHEN rn = 1 THEN MemberCount END) - " +
                "               MAX(CASE WHEN rn = 2 THEN MemberCount END)) * 100.0 / " +
                "               MAX(CASE WHEN rn = 2 THEN MemberCount END), 2) " +
                "   END AS GrowthRatePercent " +
                "FROM (SELECT *, " +
                "      ROW_NUMBER() OVER (ORDER BY Month DESC) AS rn " +
                "      FROM MonthlyMembers) " +
                "AS Ranked;";
            var ds = conn.ExecuteQueryData(query, CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                int prevCount = Convert.ToInt32(row["PrevCount"]);
                double growthRatePercent = Convert.ToDouble(row["GrowthRatePercent"]);
                if (growthRatePercent > 0)
                {
                    return "↑ " + growthRatePercent.ToString("0.00") + "% so với tháng trước";
                }
                else if (growthRatePercent < 0)
                {
                    return "↓ " + Math.Abs(growthRatePercent).ToString("0.00") + "% so với tháng trước";
                }
                else
                {
                    return "Không thay đổi so với tháng trước";
                }
            }
            else
            {
                return "Không có dữ liệu";
            }
        }
        //public string GetRatioTrainers()
        //{
        //    string query = "WITH MonthlyTrainers AS (" +
        //        "SELECT " +
        //        "   FORMAT(join_date, 'yyyy-MM') AS Month," +
        //        "   COUNT(*) AS TrainerCount " +
        //        "FROM trainers " +
        //        "GROUP BY FORMAT(join_date, 'yyyy-MM')), " +
        //        "TopTwoMonths AS (" +
        //        "SELECT TOP 2 *" +
        //        "FROM MonthlyTrainers " +
        //        "ORDER BY Month DESC) " +
        //        "SELECT " +
        //        "   MAX(CASE WHEN rn = 1 THEN Month END) AS CurrentMonth, " +
        //        "   MAX(CASE WHEN rn = 1 THEN TrainerCount END) AS CurrentCount, " +
        //        "   MAX(CASE WHEN rn = 2 THEN TrainerCount END) AS PrevCount, " +
        //        "   CASE " +
        //        "       WHEN MAX(CASE WHEN rn = 2 THEN TrainerCount END) IS NULL THEN NULL " +
        //        "       WHEN MAX(CASE WHEN rn = 2 THEN TrainerCount END) = 0 THEN NULL " +
        //        "       ELSE ROUND(" +
        //        "               (MAX(CASE WHEN rn = 1 THEN TrainerCount END) - " +
        //        "               MAX(CASE WHEN rn = 2 THEN TrainerCount END)) * 100.0 / " +
        //        "               MAX(CASE WHEN rn = 2 THEN TrainerCount END), 2) " +
        //        "   END AS GrowthRatePercent " +
        //        "FROM (SELECT *, " +
        //        "      ROW_NUMBER() OVER (ORDER BY Month DESC) AS rn " +
        //        "      FROM MonthlyTrainers) AS Ranked;";
        //    var ds = conn.ExecuteQueryData(query, CommandType.Text);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        var row = ds.Tables[0].Rows[0];
        //        int prevCount = Convert.ToInt32(row["PrevCount"]);
        //        double growthRatePercent = Convert.ToDouble(row["GrowthRatePercent"]);
        //        if (growthRatePercent > 0)
        //        {
        //            return "↑" + growthRatePercent.ToString("0.00") + "% so với tháng trước";
        //        }
        //        else if (growthRatePercent < 0)
        //        {
        //            return "↓" + Math.Abs(growthRatePercent).ToString("0.00") + "% so với tháng trước";
        //        }
        //        else
        //        {
        //            return "Không thay đổi so với tháng trước";
        //        }
        //    }
        //    return "Không có dữ liệu";
        //}
        //public string GetRatioRevenue()
        //{
        //    string query = "WITH MonthlyRevenue AS (" +
        //        "SELECT " +
        //        "   FORMAT(transaction_date, 'yyyy-MM') AS Month," +
        //        "   SUM(total_amount) AS TotalRevenue " +
        //        "FROM transactions " +
        //        "GROUP BY FORMAT(transaction_date, 'yyyy-MM')), " +
        //        "TopTwoMonths AS (" +
        //        "SELECT TOP 2 *" +
        //        "FROM MonthlyRevenue " +
        //        "ORDER BY Month DESC) " +
        //        "SELECT " +
        //        "   MAX(CASE WHEN rn = 1 THEN Month END) AS CurrentMonth, " +
        //        "   MAX(CASE WHEN rn = 1 THEN TotalRevenue END) AS CurrentCount, " +
        //        "   MAX(CASE WHEN rn = 2 THEN TotalRevenue END) AS PrevCount, " +
        //        "   CASE " +
        //        "       WHEN MAX(CASE WHEN rn = 2 THEN TotalRevenue END) IS NULL THEN NULL " +
        //        "       WHEN MAX(CASE WHEN rn = 2 THEN TotalRevenue END) = 0 THEN NULL " +
        //        "       ELSE ROUND(" +
        //        "               (MAX(CASE WHEN rn = 1 THEN TotalRevenue END) - " +
        //        "               MAX(CASE WHEN rn = 2 THEN TotalRevenue END)) * 100.0 / " +
        //        "               MAX(CASE WHEN rn = 2 THEN TotalRevenue END), 2) " +
        //        "   END AS GrowthRatePercent " +
        //        "FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY Month DESC) AS rn FROM MonthlyRevenue) AS Ranked;";
        //    var ds = conn.ExecuteQueryData(query, CommandType.Text);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        var row = ds.Tables[0].Rows[0];
        //        int prevCount = Convert.ToInt32(row["PrevCount"]);
        //        double growthRatePercent = Convert.ToDouble(row["GrowthRatePercent"]);
        //        if (growthRatePercent > 0)
        //        {
        //            return "↑" + growthRatePercent.ToString("0.00") + "% so với tháng trước";
        //        }
        //        else if (growthRatePercent < 0)
        //        {
        //            return "↓" + Math.Abs(growthRatePercent).ToString("0.00") + "% so với tháng trước";
        //        }
        //    }
        //    return "Không có dữ liệu";
        //}
    }
}
