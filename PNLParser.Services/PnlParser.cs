using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PNLParser.Models;

namespace PNLParser.Services
{
    public class PnlParser
    {
        private readonly ILogger<PnlParser> _logger;

        public PnlParser(ILogger<PnlParser> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method to parse input of PNL(string) to object FlightInfo
        /// </summary>
        /// <param name="pnlContent"></param>
        /// <returns></returns>
        public FlightInfo? Parse(string pnlContent)
        {
            _logger.LogInformation("Inizio parsing del file PNL...");

            var lstlines = pnlContent
                .Replace("\r", "") // Rimuove i caratteri di ritorno a capo
                .Split('\n', StringSplitOptions.RemoveEmptyEntries) //fa lo split in base agli spazi vuoti
                .ToList();

            _logger.LogDebug($"Numero di righe trovate: {lstlines.Count()}");
            //vengono inizzializzati gli oggeti flight e passenger
            var flight = new FlightInfo();

            //Controllo che il file inizi con la string PNL
            if(!(lstlines[0] == "PNL")){
                _logger.LogError("Errore parsing del file PNL...");
                return null;
            }

            //escludo la prima e ultima riga (PNL e ENDPNL) 
            foreach (var line in lstlines)
            {

                var passenger = new Passenger();

                try
                {
                    // info sul volo
                    if (Regex.IsMatch(line, @"(\w{2}\d+)/(\d{2}[A-Z]{3}) (\w{3})"))
                    {
                        var match = Regex.Match(line, @"(\w{2}\d+)/(\d{2}[A-Z]{3}) (\w{3})");
                        if (match.Success)
                        {
                            flight.FlightNumber = match.Groups[1].Value;
                            flight.FlightDate = ParseDate(match.Groups[2].Value);

                            _logger.LogInformation($"data: {flight.FlightDate.ToString()}");

                            flight.DepartureDestinationAirport = match.Groups[3].Value;

                            _logger.LogInformation($"Volo rilevato: {flight.FlightNumber}, Data: {flight.FlightDate.Day + "/" + flight.FlightDate.Month}, Aeroporto di partenza: {flight.DepartureDestinationAirport}");
                        }             
                    }

                    // info Destinazione e passeggeri
                    else if (Regex.IsMatch(line, @"-([A-Z]{3})(\d{3})([A-Z])"))
                    {
                        var match = Regex.Match(line, @"-([A-Z]{3})(\d{3})([A-Z])");
                        if (match.Success)
                        {
                            flight.DestinationAirport = match.Groups[1].Value;
                            flight.PassengerCount = Convert.ToInt32(match.Groups[2].Value);
                            flight.FlightClass = match.Groups[3].Value;

                            _logger.LogInformation($"Destinazione: {flight.DestinationAirport}, Passeggeri: {flight.PassengerCount}");
                        }
                    }
                    // Nuovo passeggero
                    else if (Regex.IsMatch(line, @"^\d[A-Z]+/"))
                    {
                        passenger = ParsePassenger(line);
                        if(passenger is not null) flight.Passengers.Add(passenger);

                        _logger.LogDebug($"Aggiunto passeggero: {passenger.LastName} {passenger.FirstName}, Tipo: {passenger.Type}");
                    }
                    // Servizi o PNR per il passeggero corrente
                    else if (line.StartsWith(".R/") || line.StartsWith(".L/"))
                    {
                        if (passenger != null)
                        {
                            var service = ParseService(line, passenger);
                            if (service != null)
                            {
                                passenger.Services.Add(service);
                                _logger.LogDebug($"Aggiunto servizio: {service.Code} - {service.Description}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Errore durante il parsing della riga {line}");
                }
            }

            return flight;
        }

        /// <summary>
        /// Method to parse date from string to object DateDDMM
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        private DateDDMMM ParseDate(string dateStr)
        {
            int day = int.Parse(dateStr.Substring(0, 2)); // Prende i primi 2 caratteri (giorno)
            string month = dateStr.Substring(2, 3).ToUpper(); // Prende i successivi 3 caratteri (mese)

            return new DateDDMMM { Day = day, Month = month };
        }

        /// <summary>
        /// Method to parse date from string to object Passenger
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Passenger ParsePassenger(string line)
        {
            var match = Regex.Match(line, @"\d([A-Z]+)/([A-Z]+)?-([A-Z0-9]+)?");
            if (!match.Success) return null;

            return new Passenger()
            {
                LastName = match.Groups[1].Value,
                FirstName = match.Groups[2].Value,
                Type = match.Groups[3].Success ? match.Groups[3].Value : "UNKNOWN",
            };
        }

        /// <summary>
        /// Method to parse date from string  and instance of passenger to object Passenger
        /// </summary>
        /// <param name="line"></param>
        /// <param name="passenger"></param>
        /// <returns></returns>
        private PassengerService ParseService(string line, Passenger passenger)
        {
            if (line.StartsWith(".L/"))
            {
                passenger.Pnr = line.Substring(3); // Assegna direttamente il valore PNR
                return null; // Non lo registriamo come servizio
            }
            else if (line.StartsWith(".R/"))
            {
                var parts = line.Split(" ", 2);
                return new PassengerService
                {
                    Code = parts[0], // Codice servizio (es. .R/TOP)
                    Description = parts.Length > 1 ? parts[1] : string.Empty // Descrizione
                };
            }
            return null;
        }
    }
}
