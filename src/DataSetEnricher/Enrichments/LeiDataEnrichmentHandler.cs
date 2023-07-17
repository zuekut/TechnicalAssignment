using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Enrichments;

public class LeiDataEnrichmentHandler : ILeiDataEnrichmentHandler
{
    private readonly ILogger<LeiDataEnrichmentHandler> _logger;
    private readonly IGleifRepository _gleifRepository;

    public LeiDataEnrichmentHandler(ILogger<LeiDataEnrichmentHandler> logger, IGleifRepository gleifRepository)
    {
        _logger = logger;
        _gleifRepository = gleifRepository;
    }
    public List<LeiRecord> EnrichData(List<LeiRecord> csvDataSet)
    {
        try
        {
            if (csvDataSet == null) throw new ArgumentNullException(nameof(csvDataSet));
            if (csvDataSet.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(csvDataSet));
            ILeiData originalData = new OriginalLeiData(csvDataSet);
            var enrichedDataBuilder = new LeiDecoratorBuilder(originalData)
                .WithDecorator(leiData => new GleifExtensionDecorator(leiData, _gleifRepository))
                .WithDecorator(leiData => new LeiTransactionCostCalculationDecorator(leiData, _gleifRepository))
                .Build();
            return enrichedDataBuilder.GetData();
        }
        catch (Exception exception)
        {
            throw new LeiDataEnrichmentException($"An error occured enriching the lei data: {exception.Message}", exception.InnerException);
        }
    }
}
