using CardanoAssignment.Models;

namespace CardanoAssignment.Convertors;

public interface ICsvToDataModelConvertor
{
    List<CsvDataSet> ConvertCsv(Stream csvStream);
}