using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Enrichments.Decorators;

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
            var externalLeiRecordData = externalLeiRecord?.Data;
            if (externalLeiRecordData != null)
            {
                leiRecord.GleifRecord = externalLeiRecord;
                leiRecord.LegalName = externalLeiRecordData?.FirstOrDefault()?.Attributes?.Entity?.LegalName?.Name ?? string.Empty;
                leiRecord.Bic = externalLeiRecordData?.FirstOrDefault()?.Attributes?.Bic?.FirstOrDefault() ?? string.Empty;
            }

            leiRecords.Add(leiRecord);
        }

        return leiRecords;
    }
}