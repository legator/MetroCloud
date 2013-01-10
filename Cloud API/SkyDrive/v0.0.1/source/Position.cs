using Newtonsoft.Json;

namespace SkyDrive
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Position
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
