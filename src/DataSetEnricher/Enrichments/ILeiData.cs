using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments;

public interface ILeiData
{
    List<LeiRecord> GetData();
}