using CardanoAssignment.Enrichments;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Processor;

public class DataSetEnrichmentProcessor : IDataSetEnrichmentProcessor
{
    private readonly IGleifRepository _gleifRepository;

    public DataSetEnrichmentProcessor(IGleifRepository gleifRepository)
    {
        _gleifRepository = gleifRepository;
    }
    public string ProcessDataSet(List<LeiRecord> csvDataSet)
    {
        ILeiData originalData = new OriginalLeiData(csvDataSet);
        var enrichedDataBuilder = new LeiDecoratorBuilder(originalData)
            .WithDecorator(leiData => new GleifExtensionDecorator(leiData, _gleifRepository))
            .WithDecorator(leiData => new LeiTransactionCostCalculationDecorator(leiData, _gleifRepository))
            .Build();
        IEnumerable<LeiRecord> enrichedData = enrichedDataBuilder.GetData();
        //ToDO Convert enriched Records to a CSV file.
    }
}