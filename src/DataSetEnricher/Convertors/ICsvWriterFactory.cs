using CsvHelper;

namespace CardanoAssignment.Convertors;

public interface ICsvWriterFactory
{
    CsvWriter CreateCsvWriter(StringWriter stringWriter);
}