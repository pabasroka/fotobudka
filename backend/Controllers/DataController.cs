using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    [HttpGet]
    public IActionResult Get([FromBody] string FullDateTime)
    {
        return Ok(new[] { new Data() });
        // return PDF;
    }

    [HttpPost]
    public IActionResult SaveData([FromBody] Data data)
    {
        _dataService.SaveData(data);
        return Ok();
    }
}