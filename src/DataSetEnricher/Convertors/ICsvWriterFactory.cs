using CsvHelper;

namespace CardanoAssignment.Convertors;

public interface ICsvWriterFactory
{
    IWriter CreateCsvWriter(StringWriter stringWriter);
}