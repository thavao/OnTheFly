using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Flight
    {
        [JsonProperty("flightNumber")]
        [JsonPropertyName("flightNumber")]
        public int Id { get; set; }
        public int Sales { get; set; }
        public bool Status { get; set; }
    }
}
