using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol;
using OllamaSharp;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFrameworkWithOllama.Demos
{
    public static class Demo5_AgentAndMCP
    {
        //Start local mcp server(mcphost -m ollama:qwen2.5-coder:1.5b --config "Local path of config.json file")
        public static async Task RunAsync()
        {
            // 1. Configure the command and parameters to run the third-party MCP server
            var transportOptions = new StdioClientTransportOptions
            {
                Name = "stdio",
                Command = "npx", // Executable to spawn the child process
                Arguments = ["-y", "@modelcontextprotocol/server-filesystem", "C:/AI Projects/Visual Studio/MCP Servers"],
                EnvironmentVariables = new Dictionary<string, string>
                {
                    { "MCP_TIMEOUT", "1800000" } // 1800 seconds in milliseconds
                }

            };

            try
            {
                // 2. Initialize the Stdio Transport (this spawns the child process)
                var transport = new StdioClientTransport(transportOptions);

                // 3. Create the MCP client session over the stdio transport
                await using var mcpClient = await McpClient.CreateAsync(transport);

                //// Adjusting read/request timeouts on the client initialization
                //var options = new McpClientOptions
                //{
                //   InitializationTimeout = TimeSpan.FromSeconds(3600), // Adjust as needed
                //};
                //await using var mcpClient = await McpClient.CreateAsync(transport, options);

                // 4. Discover capabilities (Tools, Resources, Prompts) exposed by the server
                var discoveredTools = await mcpClient.ListToolsAsync();
                //var MAFTools = discoveredTools.Cast<AITool>().ToList() ;   

                // 5. Convert and inject them into the MAF Agent loop
                
                // Configure your own HttpClient with a larger timeout
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:11434"),
                    Timeout = TimeSpan.FromMinutes(30) // 10 minutes timeout
                };

                // Pass it into the OllamaApiClient
                var chatClient = new OllamaApiClient(httpClient, "qwen2.5-coder:1.5b");

                // Build MAF agent with function invocation middleware bridge
                var localMcpAgent = new ChatClientAgent(
                    chatClient,
                    tools: [.. discoveredTools.Cast<AITool>()],
                    name: "Local McpAgent",
                    instructions: "You are a specialized MAF automation assistant.Use MCP filesystem tool.",
                    description: "Gets information about a single tool from the local MCP filesystem server"
                );

                // Run a prompt
                var response = await localMcpAgent.RunAsync("Type a single tool in the mcp filesystem server");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
            }

        }





    }
}
