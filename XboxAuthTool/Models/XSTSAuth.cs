using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace XboxAuthTool.Models
{

    public partial class XstsAuth
    {
        [JsonProperty("IssueInstant")]
        public System.DateTime IssueInstant { get; set; }

        [JsonProperty("NotAfter")]
        public System.DateTime NotAfter { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("DisplayClaims")]
        public object DisplayClaims { get; set; }
    }

    public partial class XstsAuth
    {
        public static XstsAuth FromJson(string json) => JsonConvert.DeserializeObject<XstsAuth>(json, XboxAuthTool.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this XstsAuth self) => JsonConvert.SerializeObject(self, XboxAuthTool.Models.Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
