
using System.Security.Principal;

namespace TeamManager.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using TeamManager.Web.Models;


    // Implements application logic using the TeamManagerDBEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class TeamManagerDomainService : LinqToEntitiesDomainService<TeamManagerDBEntities>
    {
        private IPrincipal _user;
        public override void Initialize(DomainServiceContext context)
        {
            base.Initialize(context);
            _user = context.User;
        }

        public IQueryable<Dictionary> GetDictionaryByName(string dicName)
        {
            return ObjectContext.Dictionaries.Where(dicItem => dicItem.Type == dicName).OrderBy(dicItem => dicItem.Weight);
        }

        public IQueryable<Dictionary> GetPriorities()
        {
            return GetDictionaryByName("issue_priority");
        }

        public IQueryable<Dictionary> GetActivities()
        {
            return GetDictionaryByName("time_activity");
        }

        [RequiresRole("Administrator", "Only Administrator can add dictionary item")]
        public void InsertDictionary(Dictionary dictionary)
        {
            if ((dictionary.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dictionary, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Dictionaries.AddObject(dictionary);
            }
        }

        [RequiresRole("Administrator", "Only Administrator can update dictionary item")]
        public void UpdateDictionary(Dictionary currentDictionary)
        {
            this.ObjectContext.Dictionaries.AttachAsModified(currentDictionary, this.ChangeSet.GetOriginal(currentDictionary));
        }

        [RequiresRole("Administrator", "Only Administrator can delete dictionary item")]
        public void DeleteDictionary(Dictionary dictionary)
        {
            if ((dictionary.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(dictionary, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Dictionaries.Attach(dictionary);
                this.ObjectContext.Dictionaries.DeleteObject(dictionary);
            }
        }

        public IQueryable<Issue> GetIssues()
        {
            return this.ObjectContext.Issues.OrderByDescending(issue => issue.CreatedOn);
        }

        public IQueryable<Issue> GetIssuesByProject(int projectId)
        {
            return ObjectContext.Issues.Include("TimeEntries").Include("AssignedUser")
                .Include("Creator").Where(issue => issue.ProjectId == projectId)
                .OrderByDescending(issue => issue.CreatedOn);
        }

        [RequiresRole("Administrator", "Manager", "Only Administrator and Manager can delete issue")]
        public void InsertIssue(Issue issue)
        {
            issue.CreatedOn = DateTime.Now;
            issue.AuthorId = Guid.Parse("39FEE52A-E0E6-427A-9888-79215CBB0218");
            if ((issue.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issue, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Issues.AddObject(issue);
            }
        }

        [RequiresRole("Administrator", "Manager", "Developer", "Only member with Developer role or greater can update issue")]
        public void UpdateIssue(Issue currentIssue)
        {
            currentIssue.UpdatedOn = DateTime.Now;
            this.ObjectContext.Issues.AttachAsModified(currentIssue, this.ChangeSet.GetOriginal(currentIssue));
        }

        [RequiresRole("Administrator", "Manager", "Only Administrator and Manager can delete issue")]
        public void DeleteIssue(Issue issue)
        {
            if ((issue.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issue, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Issues.Attach(issue);
                this.ObjectContext.Issues.DeleteObject(issue);
            }
        }

        public IQueryable<IssueStatus> GetIssueStatuses()
        {
            return this.ObjectContext.IssueStatuses;
        }

        [RequiresRole("Administrator", "Only Administrator can add issue status")]
        public void InsertIssueStatus(IssueStatus issueStatus)
        {
            if ((issueStatus.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issueStatus, EntityState.Added);
            }
            else
            {
                this.ObjectContext.IssueStatuses.AddObject(issueStatus);
            }
        }

        [RequiresRole("Administrator", "Only Administrator can update issue status")]
        public void UpdateIssueStatus(IssueStatus currentIssueStatus)
        {
            this.ObjectContext.IssueStatuses.AttachAsModified(currentIssueStatus, this.ChangeSet.GetOriginal(currentIssueStatus));
        }

        [RequiresRole("Administrator", "Only Administrator can delete issue status")]
        public void DeleteIssueStatus(IssueStatus issueStatus)
        {
            if ((issueStatus.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(issueStatus, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.IssueStatuses.Attach(issueStatus);
                this.ObjectContext.IssueStatuses.DeleteObject(issueStatus);
            }
        }

        public IQueryable<Project> GetProjects()
        {
            if (_user.Identity.IsAuthenticated)
                return ObjectContext.Projects.Include("Issues").Where(p => p.Status == 1).
                    OrderBy(p => p.CreatedOn);
            return ObjectContext.Projects.Include("Issues").Where(p => p.Status == 1 && p.IsPublic == 1)
                .OrderByDescending(p => p.CreatedOn);
        }

        [RequiresRole("Administrator", "Only Administrator can add new project")]
        public void InsertProject(Project project)
        {
            project.CreatedOn = DateTime.Now;
            project.CreatedBy = Guid.Parse("234A5BEC-0119-4AD1-90E1-A3022718F4BB");
            if ((project.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(project, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Projects.AddObject(project);
            }
        }

        [RequiresRole("Administrator", "Only Administrator can update new project")]
        public void UpdateProject(Project currentProject)
        {
            currentProject.UpdatedOn = DateTime.Now;
            this.ObjectContext.Projects.AttachAsModified(currentProject, this.ChangeSet.GetOriginal(currentProject));
        }

        [RequiresRole("Administrator", "Only Administrator can delete new project")]
        public void DeleteProject(Project project)
        {
            if ((project.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(project, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Projects.Attach(project);
                this.ObjectContext.Projects.DeleteObject(project);
            }
        }

        public IQueryable<TimeEntry> GetTimeEntries()
        {
            return this.ObjectContext.TimeEntries;
        }

        [RequiresRole("Administrator", "Manager", "Developer", "Only members with Developer role or higher can add time entries")]
        public void InsertTimeEntry(TimeEntry timeEntry)
        {
            timeEntry.CreatedOn = DateTime.Now;
            timeEntry.UserId = Guid.Parse("6FC2ED90-8D70-4EDF-BF40-6FD12325E86B");
            if ((timeEntry.EntityState != EntityState.Detached))
                ObjectContext.ObjectStateManager.ChangeObjectState(timeEntry, EntityState.Added);
            else
                ObjectContext.TimeEntries.AddObject(timeEntry);
        }

        public void UpdateTimeEntry(TimeEntry currentTimeEntry)
        {
            currentTimeEntry.UpdatedOn = DateTime.Now;
            ObjectContext.TimeEntries.AttachAsModified(currentTimeEntry, ChangeSet.GetOriginal(currentTimeEntry));
        }

        public void DeleteTimeEntry(TimeEntry timeEntry)
        {
            if ((timeEntry.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(timeEntry, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TimeEntries.Attach(timeEntry);
                this.ObjectContext.TimeEntries.DeleteObject(timeEntry);
            }
        }

        public IQueryable<Tracker> GetTrackers()
        {
            return this.ObjectContext.Trackers;
        }

        public void InsertTracker(Tracker tracker)
        {
            if ((tracker.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tracker, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Trackers.AddObject(tracker);
            }
        }

        public void UpdateTracker(Tracker currentTracker)
        {
            this.ObjectContext.Trackers.AttachAsModified(currentTracker, this.ChangeSet.GetOriginal(currentTracker));
        }

        public void DeleteTracker(Tracker tracker)
        {
            if ((tracker.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tracker, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Trackers.Attach(tracker);
                this.ObjectContext.Trackers.DeleteObject(tracker);
            }
        }

        public IQueryable<User> GetUsersByRole(string role)
        {
            return ObjectContext.GetUsersByRole(role).AsQueryable();
        }
    }
}


