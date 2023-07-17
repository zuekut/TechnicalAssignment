using System.Globalization;
using CardanoAssignment.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvWriterFactory : ICsvWriterFactory
{
    public IWriter CreateCsvWriter(StringWriter stringWriter)
    {
        var csvWriter = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
        csvWriter.Context.RegisterClassMap<InputDataSetMap>();
        return csvWriter;
    }
}