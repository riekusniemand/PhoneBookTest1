using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PhoneBookEntry
    {
        [JsonProperty(PropertyName = "Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Surname", NullValueHandling = NullValueHandling.Ignore)]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "ContactNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactNumber { get; set; }

    }
}