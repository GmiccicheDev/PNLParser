using System;
using System.IO;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PNLParser.Services;

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(configure => configure
                .AddConsole()
                .SetMinimumLevel(LogLevel.Debug))
            .AddSingleton<PnlParser>()
            .BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var parser = serviceProvider.GetRequiredService<PnlParser>();

        const string fileName = "PNL1.txt";

        string inputPath = $"C:/Users/giorg/Downloads/{fileName}";
        string outputPath = $"C:/Users/giorg/Downloads/output{fileName}.json";

        if (!File.Exists(inputPath))
        {
            logger.LogError($"File non trovato: {fileName}");
            return;
        }

        string pnlContent = File.ReadAllText(inputPath);
        var flightInfo = parser.Parse(pnlContent);

        if (flightInfo != null)
        {
            string jsonOutput = JsonConvert.SerializeObject(flightInfo, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(outputPath, jsonOutput);
            logger.LogInformation($"JSON {outputPath} generato con successo!");
        }
        else
        {
            logger.LogError($"ERRORE: JSON non generato!");
        }
    }
}