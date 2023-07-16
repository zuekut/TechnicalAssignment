using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments;

public class OriginalLeiData : ILeiData
{
    private readonly List<LeiRecord> _leiRecords;

    public OriginalLeiData(List<LeiRecord> leiRecords)
    {
        _leiRecords = leiRecords;
    }
    public List<LeiRecord> GetData()
    {
        return _leiRecords;
    }
}