using System.Collections.Generic;
using System.Threading.Tasks;
using OrpiLibrary.Models;

namespace HistoryService.Interfaces;

public interface IHistoryRepository
{
    public Task<IEnumerable<HistoryLog>> GetHistory(long infrastructureId, long take);
    public Task WriteHistory(long infrastructureId, HistoryLog log);
}