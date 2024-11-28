using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
   public class Appointment
    {
        public int Id { get; set; }

        public string? Animal { get; set; }
        public string? Doctor { get; set; }
        public string Subject { get; set; }

        public string? Description { get; set; }
        public string? ScheduleDate { get; set; }

        public string? ScheduleTime { get; set; }

        public string State { get; set; }
    }
}
