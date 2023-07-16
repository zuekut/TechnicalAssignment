using CardanoAssignment.Convertors;
using CardanoAssignment.Processor;
using Microsoft.AspNetCore.Mvc;

namespace CardanoAssignment.Controllers;

public class DataEnrichmentController : Controller
{
    private readonly ICsvToDataModelConvertor _csvToDataModelConvertor;
    private readonly IDataSetEnrichmentProcessor _dataSetEnrichmentProcessor;

    public DataEnrichmentController(ICsvToDataModelConvertor csvToDataModelConvertor, IDataSetEnrichmentProcessor dataSetEnrichmentProcessor)
    {
        _csvToDataModelConvertor = csvToDataModelConvertor;
        _dataSetEnrichmentProcessor = dataSetEnrichmentProcessor;
    }

    [HttpPost("enrichData")]
    public IActionResult EnrichLeiDataSet(IFormFile file)
    {
        if (file is not { Length: > 0 }) return BadRequest("No file was uploaded.");
        var csvDataSet = _csvToDataModelConvertor.ConvertCsv(file.OpenReadStream());
        var enrichedCsv = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);

        Response.Headers.Add("Content-Disposition", "attachment; filename=extended.csv");
        return Content(enrichedCsv, "text/csv");

    }
}