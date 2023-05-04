using System;

namespace MonitoringModule.Extensions; 

public static class MemoryConverter {
    public static double ToMegabytes(this ulong bytes) {
        return Math.Round(bytes / Math.Pow(2, 20), 2);
    } 
}