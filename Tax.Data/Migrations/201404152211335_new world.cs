namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newworld : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountPeriods", "AccountPeriodStatus_Id", "dbo.AccountPeriodStatus");
            DropForeignKey("dbo.ScheduleItems", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AccountPeriods", new[] { "AccountPeriodStatus_Id" });
            DropIndex("dbo.ScheduleItems", new[] { "User_Id" });
            DropTable("dbo.AccountPeriods");
            DropTable("dbo.AccountPeriodStatus");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.ClosedSchedules");
            DropTable("dbo.ozpbxcalls");
            DropTable("dbo.PBXCallQueues");
            DropTable("dbo.PBXInterpreterStatus");
            DropTable("dbo.PBXInterpreterTerminateCallQueues");
            DropTable("dbo.ozpbxlog");
            DropTable("dbo.PBXRegistrations");
            DropTable("dbo.PBXSessions");
            DropTable("dbo.PBXTransfers");
            DropTable("dbo.Prices");
            DropTable("dbo.ScheduleItems");
            DropTable("dbo.SurveyAnswerCodes");
            DropTable("dbo.SurveyQuestionCodes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SurveyQuestionCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Survey = c.String(nullable: false),
                        QuestionCode = c.String(nullable: false),
                        QuestionText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyAnswerCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Survey = c.String(nullable: false),
                        QuestionCode = c.String(nullable: false),
                        AnswerCode = c.String(nullable: false),
                        AnswerText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Description = c.String(),
                        Activity = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ValidityBegin = c.DateTime(nullable: false),
                        ValidityEnd = c.DateTime(nullable: false),
                        Sum = c.Single(nullable: false),
                        VAT = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXTransfers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CallIdOriginal = c.String(nullable: false),
                        CallIdTransferred = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXSessions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CallerCallId = c.String(nullable: false, maxLength: 100),
                        CalleeCallId = c.String(nullable: false, maxLength: 100),
                        CallDirection = c.Int(nullable: false),
                        CallerId = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        DialedNumber = c.String(nullable: false),
                        RingDuration = c.Time(nullable: false, precision: 7),
                        SessionID = c.String(nullable: false),
                        Source = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        StateDuration = c.Time(nullable: false, precision: 7),
                        TalkDuration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXRegistrations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Extension = c.String(nullable: false, maxLength: 100),
                        Registered = c.Boolean(nullable: false),
                        RegisterTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ozpbxlog",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Thread = c.Int(nullable: false),
                        Level = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 10),
                        Logger = c.String(nullable: false, maxLength: 100),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXInterpreterTerminateCallQueues",
                c => new
                    {
                        Extension = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Extension);
            
            CreateTable(
                "dbo.PBXInterpreterStatus",
                c => new
                    {
                        Extension = c.String(nullable: false, maxLength: 10),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Extension);
            
            CreateTable(
                "dbo.PBXCallQueues",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CallerExtension = c.String(nullable: false, maxLength: 100),
                        CalleeExtension = c.String(nullable: false, maxLength: 100),
                        CallId = c.String(nullable: false, maxLength: 200),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        FinishReason = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ozpbxcalls",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        Source = c.String(nullable: false, maxLength: 50),
                        CallerID = c.String(nullable: false, maxLength: 50),
                        Destination = c.String(nullable: false, maxLength: 50),
                        Duration = c.String(nullable: false, maxLength: 20),
                        CallState = c.String(nullable: false, maxLength: 50),
                        RecordURL = c.String(maxLength: 100),
                        Dialed = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClosedSchedules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SchedulePeriodStart = c.DateTime(nullable: false),
                        SchedulePeriodEnd = c.DateTime(nullable: false),
                        ClosureTimestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SenderUserName = c.String(),
                        ReceiverUserName = c.String(),
                        MessageText = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        SentTimestamp = c.DateTime(),
                        DeliveredTimetamp = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountPeriodStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountPeriods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PeriodBegin = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        AccountPeriodStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ScheduleItems", "User_Id");
            CreateIndex("dbo.AccountPeriods", "AccountPeriodStatus_Id");
            AddForeignKey("dbo.ScheduleItems", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AccountPeriods", "AccountPeriodStatus_Id", "dbo.AccountPeriodStatus", "Id");
        }
    }
}
