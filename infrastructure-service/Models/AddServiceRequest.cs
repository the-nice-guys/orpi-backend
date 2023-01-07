namespace OrpiLibrary.Models;

public class AddServiceRequest
{
    public long HostId { get; set; }
    public Service Service { get; set; }
}