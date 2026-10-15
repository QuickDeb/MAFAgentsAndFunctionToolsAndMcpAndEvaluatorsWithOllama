using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;
//using Microsoft.Extensions.AI.OpenAI;
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
    /// Multiple agents in a sequential workflow orchaestration.
    /// </summary>
    /// <remarks>Input text is translated into French by an agent and it's output is then translated into Spanish by another agent.
    /// The resulting output is reviewed for quality by yet another agent before passing the output to the next agent on the workflow pipeline
    /// to summarize the results.</remarks>
    public static class Demo2_MultiAgentWorkflow
    {
        public static async Task RunAsync()
        {
            IChatClient chatClient = new ChatClient
            ("gemma2:2b",
                new ApiKeyCredential("noApiKey"),
                new OpenAIClientOptions { Endpoint = new Uri("http://localhost:11434/v1") }

            ).AsIChatClient();

            // Create required agents for the workflow with chatClient, name and instructions.
            AIAgent frenchAgent = new ChatClientAgent
            (
                    chatClient,
                    name: "French Agent",
                    instructions: "Translate text  to French in a short sentence."
            );

            AIAgent SpanishAgent = new ChatClientAgent
            (
                chatClient,
                    name: "Spanish Agent",
                    instructions: "Translate text  to Spanish in a short sentence."
            );           

            AIAgent qualityReviewAgent = new ChatClientAgent
            (
                    chatClient,
                    name: "Quality Review Agent",
                    instructions: "This agent is responsible for reviewing the quality of the translations in two sentences or less."


            );

            AIAgent summaryAgent = new ChatClientAgent
            (
                    chatClient,
                    name: "Summary Agent",
                    instructions: "This agent is responsible for summarizing the translations in two sentences or less."
            );

            // Create a sequential workflow with the agents in the order they should be executed.
            Workflow workflow = AgentWorkflowBuilder.BuildSequential(frenchAgent, SpanishAgent, qualityReviewAgent, summaryAgent);

            // Convert the workflow to an AIAgent to run it.
            AIAgent workflowAgent = workflow.AsAIAgent();

            Console.Write("\nYou( Write a word or phrase or sentence to translate): ");
            string userInput = Console.ReadLine() ?? string.Empty;

            // RunAsync executes the workflow and returns the response to the user input.
            AgentResponse response = await workflowAgent.RunAsync(userInput);

            Console.WriteLine("\n");

            // Display the name of each agent and it's response text in the console.
            foreach (var message in response.Messages)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message.AuthorName);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message.Text);
            }

            Console.WriteLine();
        }
    }
}
