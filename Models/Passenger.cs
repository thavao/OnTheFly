using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Passenger
    {
        [Key]
        public string CPF { get; set; }
        public string Name { get; set; }
        public DateTime DtBirth { get; set; }
        public bool Status { get; set; }
    }
}
