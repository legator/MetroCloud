using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxnet
{
    public class Entry
    {
        public string type { get; set; }
        public string id { get; set; }
        public bool is_reply_comment { get; set; }
        public string message { get; set; }
        public string created_at { get; set; }
        public ViewCreatedByComment created_by { get; set; }
    }
}
