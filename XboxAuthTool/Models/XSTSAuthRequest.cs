using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace XboxAuthTool.Models
{


    public partial class XstsAuthRequest
    {
        [JsonProperty("RelyingParty")]
        public string RelyingParty { get; set; }

        [JsonProperty("TokenType")]
        public string TokenType { get; set; }

        [JsonProperty("Properties")]
        public Properties Properties { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("DeviceToken")]
        public string DeviceToken { get; set; }

        [JsonProperty("SandboxId")]
        public string SandboxId { get; set; }
    }

    public partial class XstsAuthRequest
    {
        public static XstsAuthRequest FromJson(string json) => JsonConvert.DeserializeObject<XstsAuthRequest>(json, XboxAuthTool.Models.Converter.Settings);
    }

    public static class Serial
    {
        public static string ToJson(this XstsAuthRequest self) => JsonConvert.SerializeObject(self, XboxAuthTool.Models.Converter.Settings);
    }

    public class Convert
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
