using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoryService.Interfaces;
using HistoryService.Models;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary.Models;

namespace HistoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryRepository _historyRepository;
    
    public HistoryController(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    [HttpGet]
    [Route("get_history")]
    public async Task<IActionResult> GetHistory(long infraId, long take)
    {
        var result = await _historyRepository.GetHistory(infraId, take);
        
        return Ok(result.OrderByDescending(x => x.Timestamp));
    }
    
    [HttpPost]
    [Route("write_history")]
    public async Task<IActionResult> WriteHistory(WriteHistoryRequest request)
    {
        await _historyRepository.WriteHistory(request.InfrastructureId, 
            new HistoryLog()
            {
                Title = request.Title,
                Message = request.Message
            });

        return Ok();
    }
}