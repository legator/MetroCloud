using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxnet
{
    public class UnSharedFileInfo
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string path { get; set; }
        public string path_id { get; set; }
        public string modified_at { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public object share_link { get; set; }
        public string etag { get; set; }
        public CreatedBy create_by { get; set; }
        public ModifiedBy modified_by { get; set; }
        public OwnedBy owned_by { get; set; }
        public FolderItem parent { get; set; }
    }
}
