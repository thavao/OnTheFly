namespace Models
{
    public class Passenger
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public DateOnly DtBirth { get; set; }
        public bool Status { get; set; }
    }
}
