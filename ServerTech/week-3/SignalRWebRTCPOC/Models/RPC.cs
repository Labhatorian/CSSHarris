using Newtonsoft.Json;

namespace SignalRWebRTCPOC.Models
{
    public class RPC
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("sdp")]
        public string sdp { get; set; }
    }
}
