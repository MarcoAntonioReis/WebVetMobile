using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebVetMobile.Validations
{
  public class Validator:IValidator
    {
        public string ErrorName { get; set; } = "";
        public string ErrorEmail { get; set; } = "";
        public string ErrorTelephone { get; set; } = "";
        public string ErrorPassword { get; set; } = "";

        private const string ErrorEmptyNameMsg = "Please inform your name.";
        private const string ErrorInvalidNameMsg = "Please inform a valid name.";
        private const string ErrorEmptyEmailMsg = "Please inform your email.";
        private const string ErrorInvalidEmailMsg = "Please inform your a valid email.";
        private const string ErrorEmptyTelephoneMsg = "Please inform your  telephone.";
        private const string ErrorInvalidTelephoneMsg = "Please inform your a valid telephone.";
        private const string ErrorEmptyPasswordMsg = "Please inform your password.";
        private const string ErrorInvalidPasswordMsg = "Please inform your a valid password with 8 characters, including numbers and letters.";

        public Task<bool> Validate(string name, string email, string telephone, string password)
        {
            var isNameValid = ValidateName(name);
            var isEmailValid = ValidateEmail(email);
            var isTelephoneValid = ValidateTelephone(telephone);
            var isPasswordValid = ValidatePassword(password);

            return Task.FromResult(isNameValid && isEmailValid && isTelephoneValid && isPasswordValid);
        }


        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ErrorName = ErrorEmptyNameMsg;
                return false;
            }

            if (name.Length < 3)
            {
                ErrorName = ErrorInvalidNameMsg;
                return false;
            }

            ErrorName = "";
            return true;
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ErrorEmail = ErrorEmptyEmailMsg;
                return false;
            }

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                ErrorEmail = ErrorInvalidEmailMsg;
                return false;
            }

            ErrorEmail = "";
            return true;
        }

        private bool ValidateTelephone(string telephone)
        {
            if (string.IsNullOrEmpty(telephone))
            {
                ErrorTelephone = ErrorEmptyTelephoneMsg;
                return false;
            }

            if (telephone.Length < 12)
            {
                ErrorTelephone = ErrorInvalidTelephoneMsg;
                return false;
            }

            ErrorTelephone = "";
            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ErrorPassword = ErrorEmptyPasswordMsg;
                return false;
            }

            if (password.Length < 8 || !Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"\d"))
            {
                ErrorPassword = ErrorInvalidPasswordMsg;
                return false;
            }

            ErrorPassword = "";
            return true;
        }


    }
}
