using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public interface ICsvReaderFactory
{
    IReader CreateCsvReader(TextReader reader);
 
}