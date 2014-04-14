using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Tax.Data.Models
{
    /// <summary>
    /// Egy rekord mezőérték-módosításainak a táblája. 
    /// Az egyes esetekben a következőket tartalmazza:
    /// 
    /// Create: 
    ///     OldValue: null, 
    ///     NewValue: minden mező értéke a felvitelkor
    ///     
    /// Update: 
    ///     OldValue: a mező értéke módosítás előtt
    ///     NewValue: a mező értéke a módosítás után
    ///     
    /// Delete: 
    ///     Oldvalue: a mezők értéke a törlést megelőzően 
    ///     NewValue: null
    ///     
    /// 
    /// 
    /// </summary>
    [Table("LogColumnChanges", Schema="DataLog")]
    public class LogColumnChange
    {
        public LogColumnChange()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Az adattábla rekordmódosításának naplója
        /// </summary>
        public Guid LogRecordChangeId { get; set; }
        public virtual LogRecordChange LogRecordChange { get; set; }

        /// <summary>
        /// A módosított mező neve
        /// </summary>
        public string Name {get; set;}

        /// <summary>
        /// A módosított mező módosítás előtti értéke
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// A módosított mező módosítás utáni értéke
        /// </summary>
        public string NewValue { get; set; }
    }

    /// <summary>
    /// A rekordok módosításainak adatai
    /// </summary>
    [Table("LogRecordChanges", Schema="DataLog")]
    public class LogRecordChange
    {
        public LogRecordChange()
        {
            this.Id = Guid.NewGuid();
            //null object pattern
            this.LogColumnChanges = new HashSet<LogColumnChange>();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// A naplózott adattábla rekordazonosítója
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// A rekord módosításának részletei mezőnként
        /// </summary>
        public virtual ICollection<LogColumnChange> LogColumnChanges { get; set; }

        /// <summary>
        /// A módosítás időpontja
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// A módosítást végző felhasználó
        /// Mivel elképzelhető olyan szituáció, hogy 
        /// a felhasználó nincs bejelentkezve, ezért
        /// ezt nem kötelező kitölteni
        /// </summary>
        /// 

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// A módosítás táblája
        /// </summary>
        public Guid DbTableId { get; set; }
        public virtual DbTable DbTable { get; set; }

        /// <summary>
        /// Módosítás típusa
        /// </summary>
        public EntityState ChangeType { get; set; }

    }

    /// <summary>
    /// A naplóban szereplő adatbázistáblák nevei
    /// </summary>
    [Table("DbTables", Schema="DataLog")]
    public class DbTable
    {
        public DbTable()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        //EF6.1-ben már lesz ilyen
        //[Index("IndexOnSchemaAndName", 1)]
        //public string Schema { get; set; }
        //[Index("IndexOnSchemaAndName", 2, IsUnique = true)]
        public string Name { get; set; }

    }

    ///// <summary>
    ///// Az adat módosítás típusa
    ///// </summary>
    //[Table("ChangeTypes", Schema = "DataLog")]
    //public enum LogRecordChangeType
    //{
    //    Create = EntityState.Added,
    //    Update = EntityState.Modified, 
    //    Delete = EntityState.Deleted
    //}

}
