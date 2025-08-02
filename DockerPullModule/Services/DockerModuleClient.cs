using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common.Requests;
using OrpiLibrary.Models.Common.Responses;

namespace DockerPullModule.Services;

public class DockerModuleClient: IDockerModuleClient
{
    private const string DockerModuleHost = "https://0.0.0.0:8080/update/pull";
    private readonly string _serverIpAddress;
    
    public DockerModuleClient(string serverIpAddress)
    {
        _serverIpAddress = serverIpAddress;
    }

    public async Task<IReadOnlyList<PullUpdateResponse>> GetUpdates()
    {
        using var client = new HttpClient();
        var request = new PullUpdateRequest { ServerIpAddress = _serverIpAddress };
        var response = await client.PostAsync(DockerModuleHost, new StringContent(JsonSerializer.Serialize(request)));
        return JsonSerializer.Deserialize<List<PullUpdateResponse>>(await response.Content.ReadAsStringAsync())!;
    }
}