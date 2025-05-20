using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagemement
{
    public class payment
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public int Total_Amount { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }
    }
}
