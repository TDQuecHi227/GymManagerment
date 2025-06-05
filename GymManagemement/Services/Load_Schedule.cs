using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagemement.Connection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace GymManagemement.Services
{
    public class Load_Schedule
    {
        ConnDB dB = new ConnDB();
        public List<Schedule> GetSchedule()
        {
            List<Schedule> scheduleList = new List<Schedule>();
            string query = @"SELECT * FROM trainer_schedule";
            var ds = dB.ExecuteQueryData(query, System.Data.CommandType.Text);
            foreach(DataRow data in ds.Tables[0].Rows)
            {
                Schedule schedule = new Schedule
                {
                    scheduleId = Convert.ToInt32(data["schedule_id"]),
                    trainerId = Convert.ToInt32(data["trainer_id"]),
                    dayofWeek = data["day_of_week"].ToString(),
                    startTime = TimeSpan.Parse(data["start_time"].ToString()),
                    endTime = TimeSpan.Parse(data["end_time"].ToString())
                };
                scheduleList.Add(schedule);
            }
            return scheduleList;
        }
        public List<Session> GetTrainerSessions(int trainerId)
        {
            List<Session> sessions = new List<Session>();
            string query = $@"
        SELECT ts.day_of_week, ts.start_time, ts.end_time,
               m.full_name AS memberName, m.phone
        FROM training_sessions ts
        JOIN members m ON m.member_id = ts.member_id
        WHERE ts.trainer_id = {trainerId}";

            var ds = dB.ExecuteQueryData(query, CommandType.Text);
            foreach (DataRow data in ds.Tables[0].Rows)
            {
                sessions.Add(new Session
                {
                    trainerId = trainerId,
                    dayofWeek = data["day_of_week"].ToString(),
                    startTime = TimeSpan.Parse(data["start_time"].ToString()),
                    endTime = TimeSpan.Parse(data["end_time"].ToString()),
                    memberName = data["memberName"].ToString(),
                    phone = data["phone"].ToString()
                });
            }
            return sessions;
        }
        public bool AddTrainingSession(int memberId, Schedule schedule)
        {
            string query = @"
        INSERT INTO training_sessions 
        (member_id, trainer_id, day_of_week, session_date, start_time, end_time)
        VALUES 
        (@member_id, @trainer_id, @day_of_week, @session_date, @start_time, @end_time)";
            DateTime today = DateTime.Today;
            int targetDay = 1;
            switch (schedule.dayofWeek)
            {
                case "Monday": targetDay = 1; break;
                case "Tuesday": targetDay = 2; break;
                case "Wednesday": targetDay = 3; break;
                case "Thursday": targetDay = 4; break;
                case "Friday": targetDay = 5; break;
                case "Saturday": targetDay = 6; break;
                case "Sunday": targetDay = 7; break;
            }

            DateTime sessionDate = today.AddDays(targetDay - (int)today.DayOfWeek);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@member_id", memberId);
            cmd.Parameters.AddWithValue("@trainer_id", schedule.trainerId);
            cmd.Parameters.AddWithValue("@day_of_week", schedule.dayofWeek);
            cmd.Parameters.AddWithValue("@session_date", sessionDate.Date);
            cmd.Parameters.AddWithValue("@start_time", schedule.startTime);
            cmd.Parameters.AddWithValue("@end_time", schedule.endTime);
            string err = "";
            return dB.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
        public bool UpdateTrainerForMember(int memberId, int trainerId)
        {
            string query = "UPDATE members SET trainer_id = @trainerId WHERE member_id = @memberId";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@trainerId", trainerId);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            string err = "";
            return dB.MyExecuteNonQuery(cmd, CommandType.Text, ref err);
        }
        public bool AddSchedule(Schedule schedule)
        {
            string query = @"INSERT INTO trainer_schedule (trainer_id, day_of_week, start_time, end_time) 
                            VALUES (@trainer_id, @day_of_week, @start_time, @end_time)";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@trainer_id", schedule.trainerId);
            cmd.Parameters.AddWithValue("@day_of_week", schedule.dayofWeek);
            cmd.Parameters.AddWithValue("@start_time", schedule.startTime);
            cmd.Parameters.AddWithValue("@end_time", schedule.endTime);
            string err = "";
            return dB.MyExecuteNonQuery(cmd, CommandType.Text, ref err);

        }
    }
}
