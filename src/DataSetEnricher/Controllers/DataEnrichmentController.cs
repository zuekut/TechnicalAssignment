using CardanoAssignment.Models;
using CardanoAssignment.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CardanoAssignment.Controllers;

public class DataEnrichmentController : Controller
{
    private readonly IGleifRepository _gleifRepository;

    public DataEnrichmentController(IGleifRepository gleifRepository)
    {
        _gleifRepository = gleifRepository;
    }

    [HttpPost]
    public IActionResult EnrichLeiDataSet([FromForm] LeiInputDataSet dataSet)
    {
        return Ok();
    }
}