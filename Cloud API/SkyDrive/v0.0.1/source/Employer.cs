using Newtonsoft.Json;

namespace SkyDrive
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Employer
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
