using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tax.Data.Models
{

    public class AccountPeriod
    {
        public AccountPeriod()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime PeriodBegin { get; set; }
        public DateTime PeriodEnd { get; set; }

        public virtual AccountPeriodStatus AccountPeriodStatus { get; set; }
    }

    public class AccountPeriodStatus
    {
        public AccountPeriodStatus()
        {
            Id = Guid.NewGuid();
            AccountPeriod = new HashSet<AccountPeriod>();
        }

        [Key]
        public Guid Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<AccountPeriod> AccountPeriod { get; set; }
    }

    public class Price
    {
        public Price()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime ValidityBegin { get; set; }
        public DateTime ValidityEnd { get; set; }
        public float Sum { get; set; }
        public float VAT { get; set; }
    }

}