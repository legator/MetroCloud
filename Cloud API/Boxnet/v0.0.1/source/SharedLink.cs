using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxnet
{
    public class SharedLink
    {
        public string password_enabled { get; set; }
        public string url { get; set; }
        public string download_url { get; set; }
        public string access { get; set; }
        public string preview_count { get; set; }
        public string download_count { get; set; }
        public string unshared_at { get; set; }
        public Permessions permessions { get; set; }
    }
}
