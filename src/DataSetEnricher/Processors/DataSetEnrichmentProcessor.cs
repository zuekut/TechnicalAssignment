using CardanoAssignment.Convertors;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;

namespace CardanoAssignment.Processors;

public class DataSetEnrichmentProcessor : IDataSetEnrichmentProcessor
{
    private readonly ILogger<DataSetEnrichmentProcessor> _logger;
    private readonly IGleifRepository _gleifRepository;
    private readonly ICsvConvertor _csvConvertor;

    public DataSetEnrichmentProcessor(ILogger<DataSetEnrichmentProcessor> logger, IGleifRepository gleifRepository, ICsvConvertor csvConvertor)
    {
        _logger = logger;
        _gleifRepository = gleifRepository;
        _csvConvertor = csvConvertor;
    }
    public string ProcessDataSet(List<LeiRecord> csvDataSet)
    {
        try
        {
            ILeiData originalData = new OriginalLeiData(csvDataSet);
            var enrichedDataBuilder = new LeiDecoratorBuilder(originalData)
                .WithDecorator(leiData => new GleifExtensionDecorator(leiData, _gleifRepository))
                .WithDecorator(leiData => new LeiTransactionCostCalculationDecorator(leiData, _gleifRepository))
                .Build();
            List<LeiRecord> enrichedData = enrichedDataBuilder.GetData();
            return _csvConvertor.ConvertToCsv(enrichedData);
        }
        catch (GleifApiException gleifApiException)
        {
            _logger.LogError(gleifApiException, gleifApiException.Message);
            throw;
        }
        catch (CsvConversionException csvConversionException)
        {
            _logger.LogError(csvConversionException, csvConversionException.Message);
            //ToDo Handle api response of the error
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occured while enriching the data set.");
            throw;
        }
    }
}
