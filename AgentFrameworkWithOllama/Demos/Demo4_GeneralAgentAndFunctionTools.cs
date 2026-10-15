using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFrameworkWithOllama.Demos
{
    /// <summary>
    /// Connecting an agent to the Ollama API using the OpenAI client. 
    /// Just a simple chat client and some function tools to get the cost and popularity rating of 
    /// a French dish by country and city respectively.
    /// </summary>
    public static class Demo4_GeneralAgentAndFunctionTools
    {
        public static async Task RunAsync() 
        {
            IChatClient chatClient = new ChatClient
                (
                    "qwen2.5-coder:1.5b",
                    new ApiKeyCredential("noApiKey"),
                    new OpenAIClientOptions { Endpoint = new Uri("http://localhost:11434/v1") }
                ).AsIChatClient();

            // Create agent with instructions , name and chatClient.
            AIAgent generalAgent = new ChatClientAgent
               (
                       chatClient,
                       name: "General",
                       instructions: "You are an assistant with general knowledge on all topics. Be concise. Donot ask questions.",
                       tools: [AIFunctionFactory.Create(get_cost_of_dish),
                                AIFunctionFactory.Create(get_popularity_rating_of_dish)]

               );


            Console.WriteLine("Type 'exit' to quit.");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("You(Ask anything or something about a french dish or it's popularity rating): ");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "exit") break;

                // This method sends  userinput to the model and returns the response.  
                var response = await generalAgent.RunAsync(userInput);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"GeneralAgent: {response.Text}");
                Console.WriteLine();

            }

                // Function to get the cost of the French dish by country
                [Description("Get the cost of the French dish by country")]
                Money get_cost_of_dish([Description("Name of country")] string country, [Description("Name of dish")] string dish)
                {
                    return country.ToLowerInvariant() == "France" && dish == "Cordon bleu" ? new Money(12) : new Money(17);

                }

                // Function to get the popularity rating of the French dish by city
                [Description("Get the popularity rating of the French dish by city")]
                PopularityRating get_popularity_rating_of_dish([Description("Name of city")] string city)
                {
                    return city.ToLowerInvariant() == "Paris" ? new PopularityRating(5) : new PopularityRating(3);

                }
            
        }

            record Money(decimal amount, string currency = "Euros");
            record PopularityRating(int number, string logo = "***");       


    }
}
