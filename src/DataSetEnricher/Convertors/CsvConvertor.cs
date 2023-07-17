using System.Globalization;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvConvertor : ICsvConvertor
{
    private readonly ILogger<CsvConvertor> _logger;
    private readonly ICsvWriterFactory _csvWriterFactory;
    private readonly ICsvReaderFactory _csvReaderFactory;

    public CsvConvertor(ILogger<CsvConvertor> logger, ICsvWriterFactory csvWriterFactory, ICsvReaderFactory csvReaderFactory)
    {
        _logger = logger;
        _csvWriterFactory = csvWriterFactory;
        _csvReaderFactory = csvReaderFactory;
    }
    public List<LeiRecord> ConvertFromCsv(Stream csvStream)
    {
        try
        {
            using (var reader = new StreamReader(csvStream))
            {
                using (var csv = _csvReaderFactory.CreateCsvReader(reader))
                {
                    csv.Context.RegisterClassMap<InputDataSetMap>();
                    return csv.GetRecords<LeiRecord>().ToList();
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    public string ConvertToCsv(List<LeiRecord> leiRecords)
    {
        try
        {
            var stringWriter = new StringWriter();
            var csvWriter = _csvWriterFactory.CreateCsvWriter(stringWriter);
            csvWriter.WriteRecords(leiRecords);
            csvWriter.Flush();
            return stringWriter.ToString();
        }
        catch (Exception exception)
        {
            _logger.LogDebug("Attempt of converting {@LeiRecords} to CSV failed.", leiRecords);
            throw new CsvConversionException($"An error occured converting {leiRecords.Count} leiRecords to CSV with the following message: {exception.Message}", exception.InnerException);
        }
    }
}