using System.Linq;

namespace TeamManager.Web.Models
{
    public partial class Issue
    {
        public double SpendedTime
        {
            get
            {
                var time = 0.0;
                if (TimeEntries != null)
                    time = TimeEntries.ToList().Sum(te => te.Hours);
                return time;
            }
        }
    }
}