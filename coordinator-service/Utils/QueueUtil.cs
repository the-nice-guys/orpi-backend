using OrpiLibrary.Models;

namespace coordinator_service.Utils;

public static class QueueUtil
{
    // method that returns graph of services in the infrastructure
    public static List<HashSet<Service>> GetDeploymentQueues(Infrastructure infrastructure)
    {
        List<Service> services = new List<Service>();
        foreach (var host in infrastructure.Hosts)
        {
            foreach (var service in host.Services)
            {
                services.Add(service);
            }
        }
        
        // build the graph
        Dictionary<Service, List<Service?>> graph = new Dictionary<Service, List<Service?>>();
        foreach (var service in services)
        {
            List<Service> dependencies = new List<Service>();
            foreach (var dependency in service.Dependencies)
            {
                dependencies.Add(services.Find(s => s.Name == dependency));
            }
            graph.Add(service, dependencies);
        }
        
        // separate graph into connected components
        List<HashSet<Service>> connectedComponents = FindConnectedComponents(graph);

        List<List<Service>> orderedServices = new List<List<Service>>();
        

        return connectedComponents;
    }
    
    // method that return separate connected components of the graph
    static List<HashSet<Service>> FindConnectedComponents(Dictionary<Service, List<Service>> graph)
    {
        List<HashSet<Service>> components = new List<HashSet<Service>>();
        HashSet<Service> visited = new HashSet<Service>();

        foreach (var node in graph.Keys)
        {
            if (!visited.Contains(node))
            {
                HashSet<Service> component = new HashSet<Service>();
                DFS(node, visited, component, graph);
                components.Add(component);
            }
        }

        return components;
    }
    
    private static void DFS(Service v, HashSet<Service> visited, HashSet<Service> component, Dictionary<Service, List<Service>> adjacencyList)
    {
        visited.Add(v);
        component.Add(v);

        foreach (var neighbor in adjacencyList[v])
        {
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, visited, component, adjacencyList);
            }
        }
    }
}