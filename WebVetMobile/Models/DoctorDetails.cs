using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
  public  class DoctorDetails
    {
        public int Id { get; set; }

        public string? FullName { get; set; }


        public int? Room { get; set; }


        public string? Speciality { get; set; }
    }
}
