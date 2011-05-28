using System;
using System.Windows.Media;
using Microsoft.Practices.Prism.Events;

namespace TeamManager.Infrastructure.Messages
{
    public class ActivityMessage : CompositePresentationEvent<ActivityMessageArgs>
    {
        
    }

    public class ActivityMessageArgs
    {
        public string Type { get; set; }
        public Color Color { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
    }
}