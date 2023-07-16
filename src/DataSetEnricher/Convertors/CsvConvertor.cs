using System.Globalization;
using CardanoAssignment.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvConvertor : ICsvConvertor
{
    private readonly ICsvWriterFactory _csvWriterFactory;

    public CsvConvertor(ICsvWriterFactory csvWriterFactory)
    {
        _csvWriterFactory = csvWriterFactory;
    }
    public List<LeiRecord> ConvertFromCsv(Stream csvStream)
    {
        using (var reader = new StreamReader(csvStream))
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true 
            };

            //ToDo create a CsvReaderFactory (optional)
            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Context.RegisterClassMap<InputDataSetMap>();
                return csv.GetRecords<LeiRecord>().ToList();
            }
        }
    }

    public string ConvertToCsv(List<LeiRecord> leiRecords)
    {
        var stringWriter = new StringWriter();
        var csvWriter = _csvWriterFactory.CreateCsvWriter(stringWriter);
        csvWriter.WriteRecords(leiRecords);
        csvWriter.Flush();
        return stringWriter.ToString();
    }
}