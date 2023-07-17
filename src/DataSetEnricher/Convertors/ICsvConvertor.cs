using CardanoAssignment.Models;

namespace CardanoAssignment.Convertors;

public interface ICsvConvertor
{
    List<LeiRecord> ConvertFromCsv(Stream csvStream);
    string ConvertToCsv(List<LeiRecord> leiRecords);
}