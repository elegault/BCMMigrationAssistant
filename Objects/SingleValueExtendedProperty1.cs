using Newtonsoft.Json;

namespace BCM_Migration_Tool.Objects
{
    public class SingleValueExtendedProperty
    {
        [JsonProperty("PropertyId")]
        public string PropertyId;
        [JsonProperty("Value")]
        public string Value;
    }
}
