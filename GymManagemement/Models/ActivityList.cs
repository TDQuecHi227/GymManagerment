using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagemement.Activities;

namespace GymManagemement.Models
{
    public static class ActivityList
    {
        public static List<ActivityItem> activities { get; } = new List<ActivityItem>();
    }
}
