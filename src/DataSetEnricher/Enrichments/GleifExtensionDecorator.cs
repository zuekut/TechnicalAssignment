using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Enrichments;

public class GleifExtensionDecorator : LeiDataDecorator
{
    private readonly IGleifRepository _gleifRepository;

    public GleifExtensionDecorator(ILeiData decoratedData, IGleifRepository gleifRepository) : base(decoratedData)
    {
        _gleifRepository = gleifRepository;
    }

    public override IEnumerable<LeiRecord> GetData()
    {
        foreach (var leiRecord in _decoratedData.GetData())
        {
            var externalLeiRecord = _gleifRepository.GetLeiRecordByLeiNumber(leiRecord.Lei).Result;
            leiRecord.GleifRecord = externalLeiRecord;
            yield return leiRecord;
        }
    }
}