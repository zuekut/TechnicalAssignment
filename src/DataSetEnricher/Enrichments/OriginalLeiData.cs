using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments;

public class OriginalLeiData : ILeiData
{
    private readonly IEnumerable<LeiRecord> _leiRecords;

    public OriginalLeiData(IEnumerable<LeiRecord> leiRecords)
    {
        _leiRecords = leiRecords;
    }
    public IEnumerable<LeiRecord> GetData()
    {
        return _leiRecords;
    }
}