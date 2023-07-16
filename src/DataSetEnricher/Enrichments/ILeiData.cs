using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments;

public interface ILeiData
{
    IEnumerable<LeiRecord> GetData();
}