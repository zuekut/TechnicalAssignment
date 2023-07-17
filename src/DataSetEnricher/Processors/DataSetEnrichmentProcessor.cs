using CardanoAssignment.Convertors;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;

namespace CardanoAssignment.Processors;

public class DataSetEnrichmentProcessor : IDataSetEnrichmentProcessor
{
    private readonly ILogger<DataSetEnrichmentProcessor> _logger;
    private readonly ILeiDataEnrichmentHandler _leiDataEnrichmentHandler;
    private readonly ICsvConvertor _csvConvertor;

    public DataSetEnrichmentProcessor(ILogger<DataSetEnrichmentProcessor> logger, ILeiDataEnrichmentHandler leiDataEnrichmentHandler, ICsvConvertor csvConvertor)
    {
        _logger = logger;
        _leiDataEnrichmentHandler = leiDataEnrichmentHandler;
        _csvConvertor = csvConvertor;
    }
    public string ProcessDataSet(List<LeiRecord> csvDataSet)
    {
        try
        {
            List<LeiRecord> enrichedData = _leiDataEnrichmentHandler.EnrichData(csvDataSet);
            return _csvConvertor.ConvertToCsv(enrichedData);
        }
        catch (LeiDataEnrichmentException leiDataEnrichmentException)
        {
            _logger.LogError(leiDataEnrichmentException, leiDataEnrichmentException.Message);
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
