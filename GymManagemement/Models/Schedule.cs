using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagemement
{
    public class Schedule
    {
        public int scheduleId { get; set; }
        public int trainerId { get; set; }
        public string dayofWeek { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
    }
}
