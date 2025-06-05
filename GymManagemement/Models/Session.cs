using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagemement
{
    public class Session
    {
        public int trainerId { get; set; }
        public string dayofWeek { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        // Thêm các thông tin member
        public string memberName { get; set; }
        public string phone { get; set; }
    }
}
