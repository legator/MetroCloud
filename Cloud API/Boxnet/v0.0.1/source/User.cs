using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxnet
{
    public class User
    {
        public string login { get; set; }
        public string email { get; set; }
        public string access_id { get; set; }
        public string user_id { get; set; }
        public string space_amount { get; set; }
        public string space_used { get; set; }
        public string max_upload_size { get; set; }
    }
}
