using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Tax.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email cím")]
        public string Email { get; set; }
        public bool isLocked { get; set; }
        public bool isEmailValidated { get; set; }
        public string Password { get; set; }
        public virtual KontaktUser KontaktUser { get; set; }
        public virtual SinoszUser SinoszUser { get; set; }
        public bool isSynced { get; set; }
    }

    public class KontaktUser
    {
        public KontaktUser()
        {
            Id = Guid.NewGuid();
            DeviceLog = new HashSet<DeviceLog>();
            Reservation = new HashSet<Reservation>();
        }

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isSinoszMember { get; set; }
        public string SinoszId { get; set; }
        public bool isCommunicationRequested { get; set; }
        public bool isDeviceReqested { get; set; }
        public DateTime? BirthDate { get; set; }
        public virtual ICollection<DeviceLog> DeviceLog { get; set; }
        public virtual PreregistrationData PreregistrationData { get; set; }
        public bool isElected { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }        
    }

    /// <summary>
    /// A jelszó reset és e-mail cím érvényesítés-hez szükséges 
    /// tokenek táblája. Azért ilyen, mert a GUID több vélemény 
    /// szerint nem elég jó.
    /// http://stackoverflow.com/a/698879/208922
    /// </summary>
    public class Token
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Token()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Token"))
            {
                log.Info("begin");
                //Azonosító
                Id = Guid.NewGuid();
                //Érvényesség vége
                DateTime.Now.AddDays(1);
                log.Info("end");
            }
        }

        public Token(ApplicationDbContext db, TokenTargets TokenTarget, string TargetId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Token"))
            {
                log.Info("begin");
                log.Info(string.Format("TokenTarget: {0}, TargetId: {1}", TokenTarget, TargetId));
                var toDispose = false;
                if (null == db)
                {
                    db = new ApplicationDbContext();
                    toDispose = true;
                }

                this.Id = Guid.NewGuid();
                this.TargetId = TargetId;
                this.TokenTarget = TokenTarget;
                this.Code = GetUniqueCode(x => x.Code, db);
                switch (TokenTarget)
                {
                    case TokenTargets.EmailValidation:
                        //email validáció: örökre
                        this.ValidUntil = DateTime.MaxValue;
                        break;
                    case TokenTargets.PasswordReset:
                        //jelszóreset: 1 nap
                        this.ValidUntil = DateTime.Now.AddDays(1);
                        break;
                    default:
                        //minden más: 1nap
                        this.ValidUntil = DateTime.Now.AddDays(1);
                        break;
                }

                log.Info("entry created");
                db.Entry(this).State = EntityState.Added;
                log.Info("entry added");
                db.SaveChanges();
                log.Info("entry saved");
                if (toDispose)
                {
                    log.Info("toDispose");
                    db.Dispose();
                    db = null;
                }
                log.Info("end");
            }
        }

        //public static Token GetNewToken(ApplicationDbContext db, string TargetId)
        //{
        //    var toDispose = false;
        //    if (null == db)
        //    {
        //        db = new ApplicationDbContext();
        //        toDispose = true;
        //    }

        //    var token = new Token()
        //    {
        //        Id = Guid.NewGuid(),
        //        TargetId = TargetId,
        //        Code = GetUniqueCode(x => x.Code, db),
        //        ValidUntil = DateTime.Now.AddDays(1)
        //    };
        //    db.Entry(token).State = EntityState.Added;
        //    db.SaveChanges();
        //    if (toDispose)
        //    {
        //        db.Dispose();
        //        db = null;
        //    }
        //    return token;
        //}

        /// <summary>
        /// Készít egy valóban random karakterkódot az angol ABC
        /// nagybetűiből
        /// </summary>
        /// <param name="length">a kód hossza karakterben</param>
        /// <returns>véletlen nagybetűs angol karakterekből álló kódszó</returns>

        /// <summary>
        /// Egy olyan véletlenszerű szöveget készít, ami még nem szerepel a 
        /// a Tokens tábla megadott mezőjében
        /// </summary>
        /// <param name="KeySelector">melyik mezőre biztosítjuk az egyediséget</param>
        /// <returns>véletlen nagybetűs angol karakterekből álló kódszó</returns>
        /// 


        private static string GetUniqueCode(Expression<Func<Token, string>> KeySelector, ApplicationDbContext db, int length = 10)
        {
            var result = string.Empty;
            //if (null == db)
            //{
            //    db = new ApplicationDbContext();
            //}
            //using (db)
            //{
                //vigyázunk, hogy ilyen code ne legyen több
                //nincs rá sok esély, de biztos, ami biztos.
                //azért ilyen bonyolultnak látszó, hogy paraméterezhessük a mezőt, 
                //amire az egyediséget biztosítani akarjuk
                //http://stackoverflow.com/questions/5075484/property-selector-expressionfunct-how-to-get-set-value-to-selected-property
                //Func<Customer,string> func = selector.Compile();
                //then you can access func(customer). Assigning is trickier; 
                //for simple selectors your could hope that you can simply decompose to:
                //var prop = (PropertyInfo)((MemberExpression)selector.Body).Member;
                //var func = KeySelector.Compile();

                var instanceParameter = Expression.Parameter(Type.GetType("Tax.Data.Models.Token"), "instance");
                var memberExpression = (MemberExpression)KeySelector.Body;
                var property = (PropertyInfo)memberExpression.Member;

                BinaryExpression EqualsTo;
                Expression<Func<Token, bool>> lambdaGet;
                var isDuplicate = false;

                while (
                    string.IsNullOrEmpty(result)
                    || isDuplicate
                    )
                {
                    result = NewCode(length);
                    //isDuplicate = db.Licenses.Any(x => func(x) == result);
                    EqualsTo = Expression.Equal(
                                            Expression.Property(instanceParameter, property),
                                            Expression.Constant(result, Type.GetType("System.String")));
                    lambdaGet = Expression.Lambda<Func<Token, bool>>(EqualsTo, instanceParameter);
                    lambdaGet.Compile();
                    isDuplicate = db.Tokens.Any(lambdaGet);
                }
                return result;
            //}
        }

        public static string NewCode(int length = 10)
        {
            //http://stackoverflow.com/a/698879/208922
            var token = new StringBuilder();
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                var data = new byte[4];
                for (int i = 0; i < length; i++)
                {
                    rngCsp.GetBytes(data);
                    var randomchar = Convert.ToChar(BitConverter.ToUInt32(data, 0) % 26 + 65); //Convert.ToInt32("A")==65
                    token.Append(randomchar);
                }
            }
            return token.ToString();
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Érvényesség vége
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// Érvényesítés dátuma. Ha nincs kitöltve, akkor még nem használtuk fel.
        /// </summary>
        public DateTime? ValidateDate { get; set; }

        /// <summary>
        /// Token azonosító
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Milyen célból bocsátottuk ki a tokent?
        /// </summary>
        public TokenTargets TokenTarget { get; set; }

        /// <summary>
        /// A cél objektum azonosítója
        /// </summary>
        public string TargetId { get; set; }

    }

    public enum TokenTargets
    {
        EmailValidation, PasswordReset
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        public virtual KontaktUserRole KontaktUserRole { get; set; }
    }

    public class KontaktUserRole
    {
        public KontaktUserRole()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public virtual PBXExtensionData PBXExtensionData { get; set; }
        public virtual InterpreterCenter InterpreterCenter { get; set; }
        public virtual Organization Organization { get; set; }
    }

    public class InterpreterCenter
    {
        public InterpreterCenter()
        {
            Id = Guid.NewGuid();
            KontaktUserRole = new HashSet<KontaktUserRole>();
        }

        [Key]
        public Guid Id { get; set; }        
        public string InterpreterCenterAddress { get; set; }
        public string ParcelNumber { get; set; }
        public virtual ICollection<KontaktUserRole> KontaktUserRole { get; set; }
        public virtual Postcode Postcode { get; set; }
        public virtual Area Area { get; set; }
    }

    public class Area
    {
        public Area()
        {
            Id = Guid.NewGuid();
            InterpreterCenter = new HashSet<InterpreterCenter>(); 
            DeviceLog = new HashSet<DeviceLog>(); 
            Postcode = new HashSet<Postcode>();
            
        }

        [Key]
        public Guid Id { get; set; }
        public string AreaName { get; set; }
        public int PhoneNumberLimit { get; set; }
        public int DeviceNumberLimit { get; set; }

        public virtual ICollection<InterpreterCenter> InterpreterCenter { get; set; }
        public virtual ICollection<DeviceLog> DeviceLog { get; set; }
        public virtual ICollection<Postcode> Postcode { get; set; }
    }

    public class Postcode
    {
        public Postcode()
        {
            Id = Guid.NewGuid();
            InterpreterCenter = new HashSet<InterpreterCenter>();
            SinoszUser = new HashSet<SinoszUser>();
            Organization = new HashSet<Organization>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<InterpreterCenter> InterpreterCenter { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
        public virtual ICollection<Organization> Organization { get; set; }
        public virtual Area Area { get; set; }
    }

    public class PBXExtensionData
    {
        public PBXExtensionData()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        //public string ExtensionID { get; set; }
        //public string Password { get; set; }
        public bool isDroped { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual PhoneNumber PhoneNumber { get; set; }
        public bool isSynced { get; set; }
        /// <summary>
        /// mellék kiosztása ekkor történt meg a felhasználónak
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// mellék ekkor került visszavonásra a felhasználótól
        /// </summary>
        public DateTime? EndTime { get; set; }
        public virtual LimitType LimitType { get; set; }
    }

    public class MessageViewModel
    {
        public string Message { get; set; }
    }

    public class KontaktRole : IdentityRole
    {
        public KontaktRole()
        {
            SubMenu = new HashSet<SubMenu>();
        }

        public virtual ICollection<SubMenu> SubMenu { get; set; }
    }

    public class PhoneNumber
    {
        public PhoneNumber()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string InnerPhoneNumber { get; set; }
        public string ExternalPhoneNumber { get; set; }
    }

    [DataContract]
    public class DeviceUsage
    {
        public DeviceUsage()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

        [DataMember]
        public string IMEI { get; set; }
        [DataMember]
        public string Serial { get; set; }
        [DataMember]
        public string OS { get; set; }
        [DataMember]
        public string OSVersion { get; set; }
        [DataMember]
        public string ClientVersion { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    public class LimitType
    {
        public LimitType()
        {
            Id = Guid.NewGuid();
            PBXExtensionData = new HashSet<PBXExtensionData>();
        }

        [Key]
        public Guid Id { get; set; }
        public string LimitName { get; set; }
        public float MaxLimitPerYear { get; set; }
        public float MinLimitPerQyear { get; set; }

        public virtual ICollection<PBXExtensionData> PBXExtensionData { get; set; }
    }

}