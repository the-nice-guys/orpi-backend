using System;
using OrpiLibrary.Models;

namespace MonitoringModule.Models;

public class LoadDataResponse
{
    public string ServiceId { get; set; }
    public long Timestamp { get; set; }
    public LoadData? LoadData { get; set; }
}