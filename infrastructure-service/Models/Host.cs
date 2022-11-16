namespace infrastructure_service.Models;

public class Host
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Ip { get; set; }
    public Status Status { get; set; }
    public List<Service>? Services { get; set; }
}