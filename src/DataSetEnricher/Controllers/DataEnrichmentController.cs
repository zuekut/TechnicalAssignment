using System.Text;
using CardanoAssignment.Convertors;
using CardanoAssignment.Processors;
using Microsoft.AspNetCore.Mvc;

namespace CardanoAssignment.Controllers;

public class DataEnrichmentController : Controller
{
    private readonly ICsvConvertor _csvConvertor;
    private readonly IDataSetEnrichmentProcessor _dataSetEnrichmentProcessor;

    public DataEnrichmentController(ICsvConvertor csvConvertor, IDataSetEnrichmentProcessor dataSetEnrichmentProcessor)
    {
        _csvConvertor = csvConvertor;
        _dataSetEnrichmentProcessor = dataSetEnrichmentProcessor;
    }

    [HttpPost("enrichData")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public IActionResult EnrichLeiDataSet(IFormFile file)
    {
        if (file is not { Length: > 0 }) return BadRequest("No file was uploaded.");
        var csvDataSet = _csvConvertor.ConvertFromCsv(file.OpenReadStream());
        var enrichedCsv = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
        
        var fileContentResult = new FileContentResult(Encoding.UTF8.GetBytes(enrichedCsv), "text/csv")
        {
            FileDownloadName = "enriched-dataset.csv"
        };

        return fileContentResult;
    }
}