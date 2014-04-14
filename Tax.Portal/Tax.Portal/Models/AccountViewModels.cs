﻿using Tax.Portal.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System;

namespace Tax.Portal.Models
{
    public class BootstrapViewModel
    {
        public bool IsValid { get; set; }
        public List<string> ErrorKeys { get; set; }

        public void Refresh(ModelStateDictionary ModelState)
        {
            IsValid = ModelState.IsValid;
            ErrorKeys = ModelState
                                .Where(x => 0 < x.Value.Errors.Count)
                                .Select(y => y.Key)
                                .ToList();
        }
    }

    public class ExternalLoginConfirmationViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Bejelentkezési név")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelenlegi jelszó")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszú legyen.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó megerősítés")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Az [Új jelszó] és az [Új jelszó megerősítése] tartalma nem egyforma")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [Display(Name = "Bejelentkezési név")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [Display(Name = "Emlékezz rám")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : AccountProfileViewModel//BootstrapViewModel
    {
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Bejelentkezési név")]
        [MustBeNotEqualToExistingUserName(ErrorMessage = "Ilyen Bejelentkezési név már van, válasszon másikat")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszú legyen", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítése")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Az [Új jelszó] és az [Új jelszó megerősítése] tartalma nem egyforma")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Email(ErrorMessage = "Az e-mail cím formátuma nem megfelelő")]
        [Display(Name = "Email cím")]
        [MustBeNotEqualToExistingUserEmail(ErrorMessage = "Ilyen e-mail cím már van, kérjük válasszon másikat")]
        public string Email { get; set; }
    }

    public class ResetPasswordStartViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email cím")]
        [Email(ErrorMessage = "Az e-mail cím nem megfelelő")]
        public string Email { get; set; }
    }

    public class ResetPasswordFinalizeViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Érvényesítőkód")]
        [TokenMustBeValid]
        public string Token { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszú legyen", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Ezt az adatot kötelező megadni")]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó megerősítése")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Az [Új jelszó] és az [Új jelszó megerősítése] tartalma nem egyforma")]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }
    }

    public class AccountProfileViewModel : BootstrapViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Családi név")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Keresztnév")]
        public string LastName { get; set; }

        [Display(Name = "SINOSZ tag vagyok")]
        public bool IsSinosz { get; set; }

        [Display(Name = "SINOSZ tagsági azonosító")]
        public string SinoszId { get; set; }

        [Display(Name = "Születési dátum")]
        //[DataType(DataType.Date, ErrorMessage = "A(z) [{0}] mezőnek dátumnak kell lennie!")]
        //[Range(typeof(DateTime), "1880.01.01", "2200.01.01", ErrorMessage = "A(z) [{0}] mezőnek dátumnak kell lennie!")]        
        [DisplayFormat(DataFormatString="{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        //[Display(Name = "Kommunikációs igény")]
        //[Display(Name = "Szeretné igénybe venni a KONTAKT Tolmácsszolgáltatást?")]
        [Display(Name = "Tesztelőnek jelentkezem!")]
        public bool IsRequestCommunication { get; set; }

        //[Display(Name = "Eszközigény")]
        [Display(Name = "Szeretnél készüléket kölcsönözni?")]
        public bool IsRequestDevice { get; set; }

        [Display(Name = "Bejelentkezési név")]
        public string LoginName { get; set; }

        [Display(Name = "Email cím")]
        public string LoginEmail { get; set; }

        [Display(Name = "Külső hívószám")]
        public string ExternalNumber { get; set; }

        [Display(Name = "Belső hívószám")]
        public string InnerNumber { get; set; }

        [Display(Name = "Használatba vett eszközök")]
        public List<string[]> Devices { get; set; }

    }

    public class PreRegisterViewModel : RegisterViewModel//BootstrapViewModel
    {
        [Display(Name = "SINOSZ tagsági azonosító")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public string PreSinoszId { get; set; }

        [Display(Name = "Születési dátum")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DisplayFormat(DataFormatString="{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public DateTime? PreBirthDate { get; set; }

        public Guid? SocialPositionId { get; set; }
        [Display(Name = "Betöltött társadalmi funkció")]
        public ICollection<MyListItem> SocialPositionList { get; set; }
                
        [Display(Name = "Munkavégzéshez?")]
        public bool IsNeedForJob { get; set; }

        [Display(Name = "Munkahely-munkakör")]
        public string Job { get; set; }

        [Display(Name = "Egészségügyi ellátáshoz?")]
        public bool IsNeedForHealth { get; set; }

        [Display(Name = "Önálló életvitelhez?")]
        public bool IsNeedForLife { get; set; }

        //[MustBeTrue(ErrorMessage = "A [{0}]-t el kell fogadni a az előregisztrációhoz!")]
        [MustBeTrue(ErrorMessage = "A [{0}]-t el kell fogadni a a regisztrációhoz!")]
        [Display(Name = "Szerződés feltételei")]
        public bool IsAcknowledge { get; set; }

    }

    public class ChangeEmailAddressStartViewModel : BootstrapViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email cím")]
        [Email(ErrorMessage = "Az e-mail cím nem megfelelő!")]
        public string Email { get; set; }
    }

    public class ChangeEmailAddressFinalizeViewModel : BootstrapViewModel
    {
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Érvényesítőkód")]
        public string Token { get; set; }
    }

}
