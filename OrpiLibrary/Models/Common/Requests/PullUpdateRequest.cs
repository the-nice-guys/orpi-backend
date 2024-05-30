using System.ComponentModel.DataAnnotations;

namespace OrpiLibrary.Models.Common.Requests;

public class PullUpdateRequest
{
    [Required] public  string ServerIpAddress { get; set; } = null!;
}
