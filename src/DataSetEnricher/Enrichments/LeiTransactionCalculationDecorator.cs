using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Enrichments;

public class LeiTransactionCostCalculationDecorator : LeiDataDecorator
{
    private readonly IGleifRepository _gleifRepository;

    public LeiTransactionCostCalculationDecorator(ILeiData decoratedData, IGleifRepository gleifRepository) : base(decoratedData)
    {
        _gleifRepository = gleifRepository;
    }

    public override List<LeiRecord> GetData()
    {
        List<LeiRecord> leiRecords = new List<LeiRecord>();
        foreach (var leiRecord in _decoratedData.GetData())
        {
            leiRecord.GleifRecord ??= _gleifRepository.GetLeiRecordByLeiNumber(leiRecord.Lei).Result;

            var gleifData = leiRecord.GleifRecord?.Data.FirstOrDefault();
            leiRecord.TransactionCost = gleifData?.Attributes.Entity.LegalAddress.Country switch
            {
                "GB" => (leiRecord.Notional * leiRecord.Rate) / 2,
                "NL" => (leiRecord.Notional * leiRecord.Rate) / 2 + 1 / leiRecord.Rate,
                _ => leiRecord.TransactionCost
            };

            leiRecords.Add(leiRecord);
        }

        return leiRecords;
    }
}