using System.Globalization;
using CardanoAssignment.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvWriterFactory : ICsvWriterFactory
{
    public CsvWriter CreateCsvWriter(StringWriter stringWriter)
    {
        var csvWriter = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
        csvWriter.Context.RegisterClassMap<InputDataSetMap>();
        csvWriter.WriteHeader<LeiRecord>();
        return csvWriter;
    }
}