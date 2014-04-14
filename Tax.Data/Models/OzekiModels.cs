using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tax.Data.Models
{
    [Table("ozpbxlog")]
    public class PBXLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int Thread { get; set; }
        [Required]
        [MaxLength(20)]
        public string Level { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string Logger { get; set; }
        [Required]
        public string Message { get; set; }

    }

    [Table("ozpbxcalls")]
    public class PBXCall
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        [MaxLength(50)]
        public string Source { get; set; }
        [Required]
        [MaxLength(50)]
        public string CallerID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Destination { get; set; }
        [Required]
        [MaxLength(20)]
        public string Duration { get; set; }
        [Required]
        [MaxLength(50)]
        public string CallState { get; set; }
        [MaxLength(100)]
        public string RecordURL { get; set; }
        [Required]
        [MaxLength(50)]
        public string Dialed { get; set; }

    }

    public enum PBXSessionState
    {
        Created = 0,
        Setup = 1,
        TransferSetup = 2,
        Ringing = 3,
        InCall = 4,
        CalleeOnHold = 5,
        CallerOnHold = 6,
        OnHold = 7,
        OnHoldInactive = 8,
        TransferRequested = 9,
        Transferring = 10,
        TransferCompleted = 11,
        TransferFailed = 12,
        CallerHungUp = 13,
        CalleeHungUp = 14,
        Redirected = 15,
        NotFound = 16,
        Busy = 17,
        Cancelled = 18,
        NotAnswered = 19,
        Error = 20,
        Aborted = 21,
    }

    public enum PBXCallDirection
    {
        Internal = 0,
        External = 1,
        Inbound = 2,
        Outbound = 3,
    }

    /// <summary>
    /// Store session data from SessionCreated (State: Created - 0) and SessionCompleted (other states) events
    /// </summary>
    public class PBXSession
    {

        //Fields not from OPSSDK ISession
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CallerCallId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CalleeCallId { get; set; }

        //Fields from OPSSDK ISession
        [Required]
        public PBXCallDirection CallDirection { get; set; }
        [Required]
        //CallerID, internal extension, or phone number (36201234567)
        public string CallerId { get; set; }
        [Required]
        ///Extension actually connected
        public string Destination { get; set; }
        [Required]
        public string DialedNumber { get; set; }
        [Required]
        public TimeSpan RingDuration { get; set; }
        [Required]
        public string SessionID { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public PBXSessionState State { get; set; }
        [Required]
        public TimeSpan StateDuration { get; set; }
        [Required]
        public TimeSpan TalkDuration { get; set; }

    }

    /// <summary>
    /// Store call transfer references between 2 calls (Session with status:created)
    /// </summary>
    public class PBXTransfer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string CallIdOriginal { get; set; }
        [Required]
        public string CallIdTransferred { get; set; }

    }

    /// <summary>
    /// Tolmács mikor aktív a rendszerben adatok tárolásához (belépés, kilépés)
    /// </summary>
    public class PBXRegistration
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Extension { get; set; }

        [Required]
        public bool Registered { get; set; }

        [Required]
        public DateTime RegisterTime { get; set; }

    }

    public class PBXCallQueue
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Hívó fél
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CallerExtension { get; set; }

        /// <summary>
        /// Hívott fél
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CalleeExtension { get; set; }

        /// <summary>
        /// Hívás azonosítója (várakozási sorba beérkező hívás)
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string CallId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public CallQueueFinishReason FinishReason { get; set; }

    }

    public enum CallQueueFinishReason
    {
        QueueLimitReached = 0,
        CallTransferred = 1,
        CallerHungUp = 2
    }

    public enum InterpreterStatus
    {
        Available, 
        Offline, 
        InCall,
        Administrating,
        Unknown
    }

    /// <summary>
    /// Tolmácsok aktuális állapota
    /// </summary>
    public class PBXInterpreterStatus
    {
        [Key]
        [MaxLength(10)]
        public String Extension { get; set; }

        [Required]
        public InterpreterStatus Status { get; set; }

    }

    /// <summary>
    /// Tolmácsok beragadása esetén adott tolmács hívásainak bontási kérelmét tartalmazza
    /// Feldolgozás után törölhető a tétel
    /// </summary>
    public class PBXInterpreterTerminateCallQueue
    {
        [Key]
        [MaxLength(10)]
        public String Extension { get; set; }

    }

}
