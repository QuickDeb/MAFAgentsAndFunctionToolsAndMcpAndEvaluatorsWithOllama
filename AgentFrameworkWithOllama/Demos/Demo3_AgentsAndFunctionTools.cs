using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ComponentModel;
using Microsoft.Extensions.VectorData;

namespace AgentFrameworkWithOllama.Demos
{
    /// <summary>
    /// Two agents in a sequential workflow orchestration with function tools.
    /// </summary>
    /// remarks>Input text is a French dish name. The first agent provides information about
    /// the dish and the second agent provides health benefits of the ingredients in the dish.</remarks>
    public static class Demo3_AgentsAndFunctionTools
    {
        public static async Task RunAsync()
        {

            Console.WriteLine("Starting the French cuisine Workflow...");
            try
            {
                IChatClient chatClient = new ChatClient
                (
                    "qwen2.5-coder:1.5b",
                    new ApiKeyCredential("noApiKey"),
                    new OpenAIClientOptions { Endpoint = new Uri("http://localhost:11434/v1")}
                ).AsIChatClient();

                // Agent instructions for the food expert agent
                string foodExpertAgentInstructions =
                    """
                1. Write a brief history of the dish in 20 words or less
                2. List the ingredients of the dish.
                3. Write a comment about the popularity of the dish world-wide in 20 words or less.      
                4 Please provide accurate and concise information.    
                """;

                // Agent instructions for the health expert agent
                string healthExpertAgentInstructions =
                    """    
                You are a helpful health assistant that outlines the health benefits of each ingredient in the French dish to the human body.
                Write everything in 50 words or less.
                Be accurate and concise.    es
                """;

                AIAgent foodExpertAgent = new ChatClientAgent
                (
                        chatClient,
                        name: "Food Expert",
                        instructions: foodExpertAgentInstructions,
                        tools: [AIFunctionFactory.Create(GetDishTheme),
                                AIFunctionFactory.Create(GetDishInformation)]

                );

                // Create a culinary expert agent with the specified instructions
                AIAgent culinaryExpertAgent = new ChatClientAgent
                (
                        chatClient,
                        name: "Health Expert",
                        instructions: healthExpertAgentInstructions

                        //tools :[AIFunctionFactory.Create(GetDishTheme),
                        //        AIFunctionFactory.Create(GetDishInformation)]      

                );

                //Create a sequential workflow and convert it to an AIAgent
                Workflow workflow = AgentWorkflowBuilder.BuildSequential(foodExpertAgent, culinaryExpertAgent);
                AIAgent workflowAgent = workflow.AsAIAgent();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Please type a French dish");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                string frenchDish = Console.ReadLine();
                Console.WriteLine();

                //AgentResponse response = await workflowAgent.RunAsync(frenchDish);

                // Spinner and progress bar implementation

                var loading = true;
                var loadingTask = Task.Run(async () =>
                {
                    string[] spinner = { "|", "/", "-", "\\" };
                    int barLenght = 100;
                    int count = 0;

                    while (loading)
                    {
                        int progress = (count % barLenght) + 1;
                        //string bar = new string('#', progress) + new string('-', barLenght - progress);
                        string bar = new string(' ', progress) + new string(' ', barLenght - progress);
                        string spin = spinner[count % spinner.Length];

                        // Assign previous color
                        var previousColor = Console.ForegroundColor;

                        // Set spinner & label to default color
                        Console.Write($"\rAgent working {spin} [");

                        // Set color for progress bar
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(bar);

                        // Restore color qnd close bracket
                        Console.ForegroundColor = previousColor;
                        Console.Write("]");

                        await Task.Delay(100);
                        count++;
                    }

                    Console.Write("\r" + new string(' ', 120) + "\r"); // Clears the line after loading is complete
                });

                // RunAsync executes the workflow and returns the response to the user input.
                AgentResponse response = await workflowAgent.RunAsync(frenchDish);

                loading = false;
                await loadingTask;

                Console.ForegroundColor = ConsoleColor.White;

                foreach (var message in response.Messages)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message.AuthorName);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(message.Text);

                    Console.WriteLine();
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


            //AgentResponse response = await workflowAgent.RunAsync(frenchDish);

            //loading = false;;
            //await loadingTask;

            //Console.ForegroundColor = ConsoleColor.White;

            //foreach (var message in response.Messages) 
            //{ 
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine(message.AuthorName);

            //    Console.ForegroundColor = ConsoleColor.Cyan;
            //    Console.WriteLine(message.Text);

            //    Console.WriteLine();
            //}
            //Console.WriteLine(response.Text);


            [Description("Gets the theme of the discussion")]
            string GetDishTheme() => $"French Cuisines";

            [Description("Gets the information of the French dish")]
            string GetDishInformation(string dishName, string dishIngredients, int numberOfDishIngredients) =>
            $"Name of dish:{dishName}\nIngredients :{dishIngredients}\nNumber of ingredients : {numberOfDishIngredients}";


            //[Description("Get the theme of the discussion")]
            //string get_theme_of_discussion() => $"French Cuisines";

            //[Description("Get the information of the French dish")]
            //string get_dish_information(string dishName, string dishIngredients, [Description("Number of ingredients in the dish")] int numberOfDishIngredients) =>
            //$"Name of dish:{dishName}\nIngredients :{dishIngredients}\nNumber of ingredients : {numberOfDishIngredients}";

        }             

    }
}
