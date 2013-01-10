using Newtonsoft.Json;

namespace SkyDrive
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Quota
    {
        [JsonProperty(PropertyName = "quota")]
        public string quota { get; set; }

        [JsonProperty(PropertyName = "available")]
        public string available { get; set; }
    }
}
