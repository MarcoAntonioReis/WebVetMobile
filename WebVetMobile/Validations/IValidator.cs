using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVetMobile.Validations
{
  public  interface IValidator
    {
        string ErrorName { get; set; }
        string ErrorEmail { get; set; }
        string ErrorTelephone { get; set; }
        string ErrorPassword { get; set; }
        Task<bool> Validate(string name, string email,
                           string telephone, string password);
    }
}
