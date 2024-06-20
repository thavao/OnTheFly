using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Passenger
    {
        [Key]
        public string CPF { get; set; }
        public string Name { get; set; }
        public DateTime DtBirth { get; set; }

        [JsonProperty("restricted")]
        public bool Status { get; set; }
    }
}
