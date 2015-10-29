using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    public class NewsMessage : IComparable<NewsMessage>
    {
        public bool Empty { get; set; }
        public string Origin { get; set; }
        public string Content { get; set; }

        public DateTime Time { get; set; }

        public override string ToString()
        {
            return Empty ? string.Empty : string.Format("[{0}] {1}", Origin, Content);
        }

        public int CompareTo(NewsMessage other)
        {
            return Time.Ticks < other.Time.Ticks ? -1 : Time.Ticks == other.Time.Ticks ? 0 : 1;
        }
    }
}
