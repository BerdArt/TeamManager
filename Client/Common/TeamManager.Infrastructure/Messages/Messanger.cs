using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace TeamManager.Infrastructure.Messages
{
    public class Messanger
    {
        public static T Get<T>() where T : EventBase, new()
        {
            var ev = (IEventAggregator) ServiceLocator.Current.GetInstance(typeof (IEventAggregator));
            return ev.GetEvent<T>();
        }
    }
}
