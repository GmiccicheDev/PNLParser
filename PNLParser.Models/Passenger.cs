namespace PNLParser.Models
{
    public class Passenger : UserBase
    {
        public string Type { get; set; } = string.Empty; // MR, MRS, CHLD, ecc.
        public string Pnr { get; set; } = string.Empty; //Passenger Name Record
        public List<PassengerService> Services { get; set; } = new();
    }
}
