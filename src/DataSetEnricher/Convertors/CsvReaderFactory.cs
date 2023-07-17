using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvReaderFactory : ICsvReaderFactory
{
    public IReader CreateCsvReader(TextReader reader)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };
        return new CsvReader(reader, csvConfig);
    }
}