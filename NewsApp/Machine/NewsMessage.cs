using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    public class NewsMessage
    {
        public bool Empty { get; set; }
        public string Origin { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return Empty ? string.Empty : string.Format("[{0}] {1}", Origin, Content);
        }
    }
}
