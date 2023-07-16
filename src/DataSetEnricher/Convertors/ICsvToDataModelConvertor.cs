using CardanoAssignment.Models;

namespace CardanoAssignment.Convertors;

public interface ICsvToDataModelConvertor
{
    List<LeiRecord> ConvertCsv(Stream csvStream);
}