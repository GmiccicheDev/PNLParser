using System.Reflection.Metadata.Ecma335;

namespace PNLParser.Models
{
    public class FlightInfo
    {
        public string FlightNumber { get; set; } = string.Empty;
        public DateDDMMM FlightDate { get; set; } = new();
        public string DepartureDestinationAirport { get; set; } = string.Empty;
        public string DestinationAirport { get; set; } = string.Empty;
        public int PassengerCount { get; set; }
        public string FlightClass { get; set; } = string.Empty; // Y = economy class
        public List<Passenger> Passengers { get; set; } = new();
    }

    public class DateDDMMM
    {
        public int Day { get; set; } 
        public string Month { get; set; } = string.Empty;

        public override string ToString()
        {
            var result = $"{Day} + / + {Month}";
            return result;
        }
    }
}
