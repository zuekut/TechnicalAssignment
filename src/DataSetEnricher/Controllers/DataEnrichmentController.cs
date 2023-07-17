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
    public IActionResult EnrichLeiDataSet(IFormFile file)
    {
        if (file is not { Length: > 0 }) return BadRequest("No file was uploaded.");
        var csvDataSet = _csvConvertor.ConvertFromCsv(file.OpenReadStream());
        var enrichedCsv = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);

        Response.Headers.Add("Content-Disposition", "attachment; filename=extended.csv");
        return Content(enrichedCsv, "text/csv");

    }
}