using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Tax.Portal.Helpers
{
    /// <summary>
    /// Validation attribute that demands that a boolean value must be true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrue : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {                        
            if (null == value || !(value is bool) || !(bool)value)
            {
                //Hibaüzenet vissza
                string message = string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, validationContext.DisplayName);
                return new ValidationResult(message);
            }
            //nincs hiba
            return ValidationResult.Success;
        }
    }
    
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            //: base("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})*$") { }
            //: base("^[0-9a-z\\.-]+@([0-9a-z-]+\\.)+[a-z]{2,4}$'") { }
            : base("^[_a-z0-9-]+(\\.[_a-z0-9-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,4})$") { }
        // Az előző: (([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5}){1,25})+)
        //          
        //          

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeNotEqualToExistingUserName : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (var db = new ApplicationDbContext())
            {
                string username = (value as string);
                if (db.Users.Any(x => x.UserName == username))
                {
                    //Hibaüzenet vissza
                    string message = string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, validationContext.DisplayName);
                    return new ValidationResult(message);
                }
                //nincs hiba
                return ValidationResult.Success;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeNotEqualToExistingUserEmail : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (var db = new ApplicationDbContext())
            {
                string email = (value as string);
                if (db.Users.Any(x => x.Email == email))
                {
                    //Hibaüzenet vissza
                    string message = string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, validationContext.DisplayName);
                    return new ValidationResult(message);
                }
                //nincs hiba
                return ValidationResult.Success;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TokenMustBeValid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (var db = new ApplicationDbContext())
            {
                string code = (value as string);
                var token = db.Tokens.SingleOrDefault(x => x.Code == code);
                var message = string.Empty;

                if (null!=token)
                {
                    //todo: kitenni validációba
                    if (0 >= token.ValidUntil.CompareTo(DateTime.Now))
                    { //érvénytelen token
                        //Hibaüzenet vissza
                        message = "Az érvényesítőkód érvényessége lejárt! ({0})";
                        message = string.Format(CultureInfo.CurrentUICulture, message, validationContext.DisplayName);
                        return new ValidationResult(message);
                    }

                    if (null != token.ValidateDate)
                    { //érvénytelen token
                      //Hibaüzenet vissza
                       message = "Az érvényesítőkódot már felhasználták, kétszer nem használható! ({0})";
                        message = string.Format(CultureInfo.CurrentUICulture, message, validationContext.DisplayName);
                        return new ValidationResult(message);
                    }

                    if (TokenTargets.PasswordReset != token.TokenTarget)
                    { //érvénytelen token
                        //Hibaüzenet vissza
                        message = "Nem megfelelő érvényesítőkód! ({0})";
                        message = string.Format(CultureInfo.CurrentUICulture, message, validationContext.DisplayName);
                        return new ValidationResult(message);
                    }
                    //OK
                    return ValidationResult.Success;
                }
                //hiba
                message = "Nincs ilyen érvényesítőkód! ({0})";
                message = string.Format(CultureInfo.CurrentUICulture, message, validationContext.DisplayName);
                return new ValidationResult(message);
            }
        }
    }
}