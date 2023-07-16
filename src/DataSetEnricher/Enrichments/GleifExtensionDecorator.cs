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

    public override List<LeiRecord> GetData()
    {
        List<LeiRecord> leiRecords = new List<LeiRecord>();
        foreach (var leiRecord in _decoratedData.GetData())
        {
            var externalLeiRecord = _gleifRepository.GetLeiRecordByLeiNumber(leiRecord.Lei).Result;
            leiRecord.GleifRecord = externalLeiRecord;
            leiRecord.LegalName = externalLeiRecord.Data.FirstOrDefault().Attributes.Entity.LegalName.Name;
            leiRecord.Bic = externalLeiRecord.Data.FirstOrDefault().Attributes.Bic.FirstOrDefault();
            leiRecords.Add(leiRecord);
        }

        return leiRecords;
    }
}