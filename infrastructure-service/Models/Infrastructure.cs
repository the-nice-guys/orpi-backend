namespace infrastructure_service.Models;

public class Infrastructure
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public List<Host>? Hosts { get; set; }
    //public dynamic Hosts { get; set; }
}