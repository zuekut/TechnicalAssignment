using CardanoAssignment.Convertors;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Processor;

public class DataSetEnrichmentProcessor : IDataSetEnrichmentProcessor
{
    private readonly IGleifRepository _gleifRepository;
    private readonly ICsvConvertor _csvConvertor;

    public DataSetEnrichmentProcessor(IGleifRepository gleifRepository, ICsvConvertor csvConvertor)
    {
        _gleifRepository = gleifRepository;
        _csvConvertor = csvConvertor;
    }
    public string ProcessDataSet(List<LeiRecord> csvDataSet)
    {
        ILeiData originalData = new OriginalLeiData(csvDataSet);
        var enrichedDataBuilder = new LeiDecoratorBuilder(originalData)
            .WithDecorator(leiData => new GleifExtensionDecorator(leiData, _gleifRepository))
            .WithDecorator(leiData => new LeiTransactionCostCalculationDecorator(leiData, _gleifRepository))
            .Build();
        List<LeiRecord> enrichedData = enrichedDataBuilder.GetData();
        return _csvConvertor.ConvertToCsv(enrichedData);
    }
}