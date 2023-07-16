using CardanoAssignment.Convertors;
using Microsoft.AspNetCore.Mvc;

namespace CardanoAssignment.Controllers;

public class DataEnrichmentController : Controller
{
    private readonly ICsvToDataModelConvertor _csvToDataModelConvertor;

    public DataEnrichmentController(ICsvToDataModelConvertor csvToDataModelConvertor)
    {
        _csvToDataModelConvertor = csvToDataModelConvertor;
    }

    [HttpPost("enrichData")]
    public IActionResult EnrichLeiDataSet(IFormFile file)
    {
        if (file is not { Length: > 0 }) return BadRequest("No file was uploaded.");
        var csvDataSet = _csvToDataModelConvertor.ConvertCsv(file.OpenReadStream());

        return Ok();

    }
}