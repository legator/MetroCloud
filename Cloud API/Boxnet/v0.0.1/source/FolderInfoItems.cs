using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxnet
{
    public class FolderInfoItems
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public object shared_link { get; set; }
        public CreatedBy created_by { get; set; }
        public ModifiedBy modified_by { get; set; }
        public OwnedBy owned_by { get; set; }
        public FolderItem parent { get; set; }
        public ItemCollections item_collection { get; set; }
    }
}
