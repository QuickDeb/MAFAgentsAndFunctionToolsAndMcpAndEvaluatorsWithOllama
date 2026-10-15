using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFrameworkWithOllama.Demos
{
    /// <summary>
    /// Connecting an agent to the Ollama API using the OpenAI client. 
    /// No MCP/function tools , just a simple chat client.
    /// </summary>
    public static class Demo1_FillmAgent
    {
        public static async Task RunAsync()
        {
            // OllamaApiClient implements IChatClient(could also be Azure OpenAI, OpenAI, etc).
            var chatClient = new OllamaApiClient(
                new Uri("http://localhost:11434/V1"), "qwen2.5:0.5b");


            Console.WriteLine("Welcome to the Ollama API Demo!");
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit.");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("You: ");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "exit") break;

                // Create agent with instructions for film expertise, name and chatClient.
                AIAgent filmAgent = new ChatClientAgent
                (
                    chatClient,
                    name: "FilmAgent",
                    instructions: """
                    You are a film expert. Be concise.
                    Answer questions about movies and directors in 2 sentences or less.
                    """
                );

                // This method sends  userinput to the model and returns the response.  
                var response = await filmAgent.RunAsync(userInput);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"FilmAgent: {response.Text}");
                Console.WriteLine();
            }

        }
    };
}
