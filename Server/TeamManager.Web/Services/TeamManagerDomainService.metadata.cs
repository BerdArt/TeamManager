
namespace TeamManager.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies ProjectMetadata as the class
    // that carries additional metadata for the Project class.
    [MetadataTypeAttribute(typeof(Project.ProjectMetadata))]
    public partial class Project
    {

        // This class allows you to attach custom attributes to properties
        // of the Project class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ProjectMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ProjectMetadata()
            {
            }

            [Editable(false)]
            [Display(Order = 0, Name = "Project ID")]
            public int Id { get; set; }

            [Display(Order = 1, Name = "Project title")]
            [StringLength(50)]
            public string Title { get; set; }

            [Display(Order = 2)]
            public string Description { get; set; }

            [Display(AutoGenerateField = false)]
            public Guid CreatedBy { get; set; }

            [Display(AutoGenerateField = false)]
            public Nullable<DateTime> CreatedOn { get; set; }

            [Editable(false)]
            [Display]
            public User Creator { get; set; }

            [Display(Order = 3, Name = "Is public")]
            public Nullable<byte> IsPublic { get; set; }

            [Include]
            [Display(AutoGenerateField = false)]
            public EntityCollection<Issue> Issues { get; set; }

            [Display(Name = "Issue status")]
            [DefaultValue(2)]
            public int Status { get; set; }

            [Display(AutoGenerateField = false)]
            public Nullable<DateTime> UpdatedOn { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies IssueMetadata as the class
    // that carries additional metadata for the Issue class.
    [MetadataTypeAttribute(typeof(Issue.IssueMetadata))]
    public partial class Issue
    {

        // This class allows you to attach custom attributes to properties
        // of the Issue class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class IssueMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private IssueMetadata()
            {
            }

            [Editable(false)]
            [Display(AutoGenerateField = false)]
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(50)]
            public string Subject { get; set; }

            public string Description { get; set; }

            [Display(AutoGenerateField = false, Name = "Project")]
            public Project Project { get; set; }

            [Display(AutoGenerateField = false, Name = "Project")]
            [Required]
            public int ProjectId { get; set; }

            [Display(Name = "Status")]
            [DefaultValue(2)]
            [Required]
            public int StatusId { get; set; }

            [Display(Name = "Status")]
            public IssueStatus IssueStatus { get; set; }

            [Display(Name = "Priority")]
            [DefaultValue(2)]
            [Required]
            public int PriorityId { get; set; }

            [Display(Name = "Priority")]
            public Dictionary Priority { get; set; }

            [Display(Name = "Assigned to")]
            public Nullable<Guid> AssignedTo { get; set; }

            [Display(Name = "Assigned to", AutoGenerateField = false)]
            public User AssignedUser { get; set; }

            [Display(AutoGenerateField = false)]
            public Guid AuthorId { get; set; }

            [Display(AutoGenerateField = false)]
            public DateTime CreatedOn { get; set; }

            [Display(AutoGenerateField = false)]
            public User Creator { get; set; }

            [Display(Name = "Done ratio")]
            [DefaultValue(0)]
            [Range(0, 100)]
            public int DoneRatio { get; set; }

            [Display(Name = "Due date")]
            public Nullable<DateTime> DueDate { get; set; }

            [Display(Name = "Estimated")]
            [DefaultValue(0)]
            public Nullable<double> EstimatedHours { get; set; }

            [Display(Name = "Start date")]
            public Nullable<DateTime> StartDate { get; set; }

            [Display(AutoGenerateField = false)]
            public EntityCollection<TimeEntry> TimeEntries { get; set; }

            [Display(Name = "Tracker")]
            [DefaultValue(2)]
            [Required]
            public int TrackerId { get; set; }

            [Display(Name = "Tracker", AutoGenerateField = false)]
            public Tracker Tracker { get; set; }

            [Display(AutoGenerateField = false)]
            public Nullable<DateTime> UpdatedOn { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies IssueStatusMetadata as the class
    // that carries additional metadata for the IssueStatus class.
    [MetadataTypeAttribute(typeof(IssueStatus.IssueStatusMetadata))]
    public partial class IssueStatus
    {

        // This class allows you to attach custom attributes to properties
        // of the IssueStatus class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class IssueStatusMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private IssueStatusMetadata()
            {
            }

            [Display(AutoGenerateField = false)]
            public int Id { get; set; }

            [Display(Name = "Status name")]
            [StringLength(50)]
            public string Name { get; set; }

            [Display(Name = "Is closed")]
            [DefaultValue(0)]
            public byte IsClosed { get; set; }

            [Display(Name = "Is default")]
            [DefaultValue(0)]
            public byte IsDefault { get; set; }

            [Display(Name = "Default done ratio")]
            [DefaultValue(0)]
            public Nullable<int> DefaultDoneRatio { get; set; }

            [Display(AutoGenerateField = false)]
            [DefaultValue(0)]
            public Nullable<int> Weight { get; set; }

            public EntityCollection<Issue> Issues { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TrackerMetadata as the class
    // that carries additional metadata for the Tracker class.
    [MetadataTypeAttribute(typeof(Tracker.TrackerMetadata))]
    public partial class Tracker
    {

        // This class allows you to attach custom attributes to properties
        // of the Tracker class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TrackerMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TrackerMetadata()
            {
            }

            [Display(AutoGenerateField = false)]
            [Key]
            public int Id { get; set; }

            public EntityCollection<Issue> Issues { get; set; }

            [Display(Name = "Tracker name")]
            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            [Display(AutoGenerateField = false)]
            [DefaultValue(0)]
            public Nullable<int> Weight { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies DictionaryMetadata as the class
    // that carries additional metadata for the Dictionary class.
    [MetadataTypeAttribute(typeof(Dictionary.DictionaryMetadata))]
    public partial class Dictionary
    {

        // This class allows you to attach custom attributes to properties
        // of the Dictionary class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DictionaryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DictionaryMetadata()
            {
            }

            [Display(AutoGenerateField = false)]
            [Key]
            public int Id { get; set; }

            [Display(Name = "Dictionary value")]
            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            [Display(Name = "Dictionary type")]
            [Required]
            [StringLength(50)]
            public string Type { get; set; }

            [Display(Name = "Is default")]
            [DefaultValue(0)]
            public byte IsDefault { get; set; }

            [Display(AutoGenerateField = false)]
            [DefaultValue(0)]
            public Nullable<int> Weight { get; set; }

            public EntityCollection<TimeEntry> TimeEntries { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TimeEntryMetadata as the class
    // that carries additional metadata for the TimeEntry class.
    [MetadataTypeAttribute(typeof(TimeEntry.TimeEntryMetadata))]
    public partial class TimeEntry
    {

        // This class allows you to attach custom attributes to properties
        // of the TimeEntry class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TimeEntryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TimeEntryMetadata()
            {
            }

            [Display(AutoGenerateField = false)]
            [Key]
            public int Id { get; set; }

            [Display(AutoGenerateField = false, Name = "Issue")]
            public Issue Issue { get; set; }

            [Display(AutoGenerateField = false)]
            [Required]
            public int IssueId { get; set; }

            [Display(AutoGenerateField = false)]
            public User User { get; set; }

            [Display(AutoGenerateField = false)]
            [Required]
            public Guid UserId { get; set; }

            [Required]
            public double Hours { get; set; }

            public string Comment { get; set; }

            [Display(AutoGenerateField = false)]
            [Required]
            public DateTime CreatedOn { get; set; }

            [Display(Name = "Date")]
            [Required]
            public DateTime SpentOn { get; set; }

            [Display(AutoGenerateField = false)]
            public Nullable<DateTime> UpdatedOn { get; set; }

            [Display(Name = "Activity")]
            [Required]
            public int ActivityId { get; set; }

            public Dictionary Activity { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UserMetadata as the class
    // that carries additional metadata for the User class.
    [MetadataTypeAttribute(typeof(User.UserMetadata))]
    public partial class User
    {

        // This class allows you to attach custom attributes to properties
        // of the User class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserMetadata()
            {
            }

            public Guid ApplicationId { get; set; }

            public EntityCollection<Issue> Assignements { get; set; }

            public bool IsAnonymous { get; set; }

            public EntityCollection<Issue> Issues { get; set; }

            public DateTime LastActivityDate { get; set; }

            public string LoweredUserName { get; set; }

            public string MobileAlias { get; set; }

            public EntityCollection<Project> Projects { get; set; }

            public EntityCollection<TimeEntry> TimeEntries { get; set; }

            public Guid UserId { get; set; }

            public string UserName { get; set; }
        }
    }
}
