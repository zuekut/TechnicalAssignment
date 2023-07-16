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

    public override IEnumerable<LeiRecord> GetData()
    {
        foreach (var leiRecord in _decoratedData.GetData())
        {
            leiRecord.GleifRecord ??= _gleifRepository.GetLeiRecordByLeiNumber(leiRecord.Lei).Result;

            var gleifData = leiRecord.GleifRecord?.Data.FirstOrDefault();
            switch (gleifData?.Attributes.Entity.LegalAddress.Country)
            {
                case "GB":
                    leiRecord.AdditionalEnrichedData.Add("transaction_costs", (leiRecord.Notional * leiRecord.Rate - leiRecord.Notional) / 2);
                    break;
                case "NL":
                    leiRecord.AdditionalEnrichedData.Add("transaction_costs", (leiRecord.Notional * leiRecord.Rate - leiRecord.Notional) / 2 + 1 / leiRecord.Rate);
                    break;
            }

            yield return leiRecord;
        }
    }
}