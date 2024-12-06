using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Models
{
  public  class ChangePasswordApi
    {

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Confirm { get; set; }
    }
}
