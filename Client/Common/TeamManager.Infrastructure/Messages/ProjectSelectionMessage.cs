using Microsoft.Practices.Prism.Events;

namespace TeamManager.Infrastructure.Messages
{
    public class ProjectSelectionMessage : CompositePresentationEvent<SelectedProjectArgs>
    { }

    public class SelectedProjectArgs
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
    }
}